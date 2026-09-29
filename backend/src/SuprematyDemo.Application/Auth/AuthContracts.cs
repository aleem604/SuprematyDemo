namespace SuprematyDemo.Application.Auth;

public sealed record SignUpRequest(string Email, string Password, string DisplayName);
public sealed record LoginRequest(string Email, string Password);
public sealed record ForgotPasswordRequest(string Email);
public sealed record ResetPasswordRequest(string Email, string Token, string NewPassword);
public sealed record UserDto(Guid Id, string Email, string DisplayName);
public sealed record AuthResponse(string AccessToken, DateTimeOffset ExpiresUtc, UserDto User);
public sealed record ForgotPasswordResponse(string Message, string? DevelopmentResetToken = null);
public sealed record AuthTokenOptions(string Issuer, string Audience, string SigningKey, int ExpiryMinutes = 60);

public interface IAuthService
{
    Task<AuthResponse> SignUpAsync(SignUpRequest request, CancellationToken ct = default);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, bool exposeDevelopmentToken, CancellationToken ct = default);
    Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default);
    Task<UserDto?> GetUserAsync(Guid userId, CancellationToken ct = default);
}
