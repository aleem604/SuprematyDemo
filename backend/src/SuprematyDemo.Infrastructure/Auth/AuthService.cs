using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SuprematyDemo.Application.Auth;
using SuprematyDemo.Domain.Entities;
using SuprematyDemo.Infrastructure.Persistence;

namespace SuprematyDemo.Infrastructure.Auth;

public sealed class AuthService(AppDbContext db, IPasswordHasher<User> hasher, AuthTokenOptions options) : IAuthService
{
    public async Task<AuthResponse> SignUpAsync(SignUpRequest request, CancellationToken ct = default)
    {
        ValidatePassword(request.Password);
        var email = NormalizeEmail(request.Email);
        if (await db.Users.AnyAsync(x => x.Email == email, ct)) throw new InvalidOperationException("An account with this email already exists.");
        var user = new User(email, request.DisplayName);
        user.SetPasswordHash(hasher.HashPassword(user, request.Password));
        db.Users.Add(user); await db.SaveChangesAsync(ct);
        return CreateAuthResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var email = NormalizeEmail(request.Email);
        var user = await db.Users.SingleOrDefaultAsync(x => x.Email == email, ct) ?? throw new UnauthorizedAccessException("Invalid email or password.");
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed) throw new UnauthorizedAccessException("Invalid email or password.");
        if (result == PasswordVerificationResult.SuccessRehashNeeded) { user.SetPasswordHash(hasher.HashPassword(user, request.Password)); await db.SaveChangesAsync(ct); }
        return CreateAuthResponse(user);
    }

    public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, bool exposeDevelopmentToken, CancellationToken ct = default)
    {
        const string message = "If an account exists for that email, password reset instructions have been generated.";
        var email = NormalizeEmail(request.Email);
        var user = await db.Users.SingleOrDefaultAsync(x => x.Email == email, ct);
        if (user is null) return new(message);
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        user.SetPasswordResetToken(HashToken(token), DateTimeOffset.UtcNow.AddMinutes(15));
        await db.SaveChangesAsync(ct);
        return new(message, exposeDevelopmentToken ? token : null);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default)
    {
        ValidatePassword(request.NewPassword);
        var email = NormalizeEmail(request.Email);
        var user = await db.Users.SingleOrDefaultAsync(x => x.Email == email, ct) ?? throw new InvalidOperationException("Invalid or expired reset token.");
        if (user.PasswordResetTokenHash is null || user.PasswordResetTokenExpiresUtc <= DateTimeOffset.UtcNow || !CryptographicOperations.FixedTimeEquals(Convert.FromHexString(user.PasswordResetTokenHash), Convert.FromHexString(HashToken(request.Token))))
            throw new InvalidOperationException("Invalid or expired reset token.");
        user.SetPasswordHash(hasher.HashPassword(user, request.NewPassword)); user.ClearPasswordResetToken(); await db.SaveChangesAsync(ct);
    }

    public async Task<UserDto?> GetUserAsync(Guid userId, CancellationToken ct = default) => await db.Users.Where(x => x.Id == userId).Select(x => new UserDto(x.Id, x.Email, x.DisplayName)).SingleOrDefaultAsync(ct);

    private AuthResponse CreateAuthResponse(User user)
    {
        var expires = DateTimeOffset.UtcNow.AddMinutes(options.ExpiryMinutes);
        var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), new Claim(JwtRegisteredClaimNames.Email, user.Email), new Claim(ClaimTypes.Name, user.DisplayName), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "Customer") };
        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(options.Issuer, options.Audience, claims, expires: expires.UtcDateTime, signingCredentials: credentials);
        return new(new JwtSecurityTokenHandler().WriteToken(token), expires, new(user.Id, user.Email, user.DisplayName));
    }
    private static string NormalizeEmail(string email) { if (string.IsNullOrWhiteSpace(email) || !email.Contains('@')) throw new ArgumentException("A valid email is required."); return email.Trim().ToLowerInvariant(); }
    private static void ValidatePassword(string password) { if (string.IsNullOrWhiteSpace(password) || password.Length < 8 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit)) throw new ArgumentException("Password must be at least 8 characters and contain upper-case, lower-case and numeric characters."); }
    private static string HashToken(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
