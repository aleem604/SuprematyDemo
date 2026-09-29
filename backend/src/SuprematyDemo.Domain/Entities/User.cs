namespace SuprematyDemo.Domain.Entities;

public sealed class User
{
    private User() { }
    public User(string email, string displayName)
    {
        Id = Guid.NewGuid(); Email = email.Trim().ToLowerInvariant(); DisplayName = displayName.Trim(); CreatedUtc = DateTimeOffset.UtcNow;
    }
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string? PasswordResetTokenHash { get; private set; }
    public DateTimeOffset? PasswordResetTokenExpiresUtc { get; private set; }
    public bool IsAdmin { get; set; }
    public DateTimeOffset CreatedUtc { get; private set; }
    public void SetPasswordHash(string hash) => PasswordHash = hash;
    public void SetPasswordResetToken(string hash, DateTimeOffset expiresUtc) { PasswordResetTokenHash = hash; PasswordResetTokenExpiresUtc = expiresUtc; }
    public void ClearPasswordResetToken() { PasswordResetTokenHash = null; PasswordResetTokenExpiresUtc = null; }
}
