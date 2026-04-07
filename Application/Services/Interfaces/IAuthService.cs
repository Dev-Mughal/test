using Application.Utilities.TokenManager;
using Common.Features.Auth.Login;
using Common.Features.Auth.RefreshToken;
using Common.Features.Auth.SignUp.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<EmailVerificationResultDto> IsEmailAlreadyExists(EmailVerificationDto dto);
        Task<TokenResponse> LoginAsync(LoginDto request);
        Task LogoutAsync();
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenDto request);
        Task<TokenResponse> SignUpAsync(SignUpDto dto, IFormFile? businessImage);
    }
}