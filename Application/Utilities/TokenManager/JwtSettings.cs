namespace Application.Utilities.TokenManager
{
    public sealed class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenExpiryMinutes { get; set; } = 120;
        public int RememberMeTokenExpiryDays { get; set; } = 30;

        // Backward compatibility with existing keys
        public int AccessTokenExpirationMinutes { get; set; } = 120;
        public int RefreshTokenExpirationDays { get; set; } = 30;
    }
}
