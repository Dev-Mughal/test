using Application.Utilities.TokenManager;
using Common.Features.Auth.Login;
using Common.Features.Auth.RefreshToken;
using Common.Features.Customer.Auth.DTOs;

namespace Application.Services.Interfaces
{
    public interface ICustomerAuthService
    {
        Task<TokenResponse> SignUpAsync(CustomerSignUpDto dto);
        Task<TokenResponse> LoginAsync(LoginDto dto);
        Task LogoutAsync();
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenDto dto);
        Task<EmailVerificationResultDto> IsEmailAlreadyExistsAsync(EmailVerificationDto dto);
        Task UpdateProfileAsync(CustomerUpdateProfileDto dto);
        Task UpdatePasswordAsync(CustomerUpdatePasswordDto dto);
    }
}
