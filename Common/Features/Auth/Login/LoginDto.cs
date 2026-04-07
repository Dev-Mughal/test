namespace Common.Features.Auth.Login
{
    public record LoginDto(string Email, string Password, bool RememberMe = false);
    public record EmailVerificationDto(string Email);
    public record EmailVerificationResultDto(bool Exists);
}
