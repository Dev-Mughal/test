using Application.Constants;
using Application.Utilities.TokenManager;
using Application.Utilities.UserContext;
using Common.Exceptions;
using Common.Features.Auth.Login;
using Common.Features.Auth.RefreshToken;
using Common.Features.Customer.Auth.DTOs;
using Domain;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Common.Models;
using Application.Services.Interfaces;

namespace Application.Services.Implementations
{
    public class CustomerAuthService(
        IDbContextFactory<BizyPopDbContext> contextFactory,
        IPasswordHasher<Customer> passwordHasher,
        ITokenService tokenService,
        IImageService imageService,
        IAuthorizedCustomer authorizedCustomer) : ICustomerAuthService
    {
        public async Task<TokenResponse> SignUpAsync(CustomerSignUpDto dto)
        {
            // Reject duplicate emails before creating the record
            var emailTaken = await contextFactory.QueryWithDbContextAsync(async context =>
                await context.Customers
                    .AnyAsync(c => c.Email.Equals(dto.Email))
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            if (emailTaken)
                throw new DuplicateResourceException($"An account with the email '{dto.Email}' already exists.");

            // Optional: save profile photo if provided
            string? profilePhotoUrl = null;
            if (dto.ProfilePhoto is { Length: > 0 })
                profilePhotoUrl = await imageService.SaveImageAsync(dto.ProfilePhoto, ImageTypeEnum.Customer)
                    .ConfigureAwait(false);

            var customer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                TimeZone = dto.TimeZone,
                ProfilePhotoUrl = profilePhotoUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PasswordHash = string.Empty   // placeholder — hashed below after the entity exists
            };

            // Hash the password against the entity instance (IPasswordHasher requires the user object)
            customer.PasswordHash = passwordHasher.HashPassword(customer, dto.Password);

            await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await context.Customers.AddAsync(customer).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            var tokens = tokenService.GenerateCustomerTokens(customer);
            await tokenService.SaveCustomerRefreshTokenAsync(customer.CustomerId, tokens.RefreshToken)
                .ConfigureAwait(false);

            return tokens;
        }

        public async Task<TokenResponse> LoginAsync(LoginDto dto)
        {
            var customer = await contextFactory.QueryWithDbContextAsync(async context =>
                await context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Email.Equals(dto.Email) && c.IsActive)
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            if (customer is null)
                throw new UnauthorizedException("Invalid credentials.");

            var verificationResult = passwordHasher.VerifyHashedPassword(
                customer, customer.PasswordHash, dto.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
                throw new UnauthorizedException("Invalid credentials.");

            var tokens = tokenService.GenerateCustomerTokens(customer, dto.RememberMe);
            await tokenService.SaveCustomerRefreshTokenAsync(customer.CustomerId, tokens.RefreshToken, dto.RememberMe)
                .ConfigureAwait(false);

            return tokens;
        }

        public async Task LogoutAsync() =>
            await tokenService.RevokeCustomerRefreshTokenAsync(authorizedCustomer.Id)
                .ConfigureAwait(false);

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var customer = await contextFactory.QueryWithDbContextAsync(async context =>
                await context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c =>
                        c.RefreshToken == dto.RefreshToken &&
                        c.RefreshTokenExpiryTime > DateTime.UtcNow &&
                        c.IsActive)
                    .ConfigureAwait(false)
            ).ConfigureAwait(false)
            ?? throw new UnauthorizedException("Invalid or expired refresh token.");

            var rememberMe = tokenService.IsRememberMeSession(customer.RefreshTokenExpiryTime);
            var tokens = tokenService.GenerateCustomerTokens(customer, rememberMe);
            await tokenService.SaveCustomerRefreshTokenAsync(customer.CustomerId, tokens.RefreshToken, rememberMe)
                .ConfigureAwait(false);

            return tokens;
        }

        public async Task<EmailVerificationResultDto> IsEmailAlreadyExistsAsync(EmailVerificationDto dto)
        {
            var exists = await contextFactory.QueryWithDbContextAsync(async context =>
                await context.Customers
                    .AnyAsync(c => c.Email.Equals(dto.Email))
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            return new EmailVerificationResultDto(exists);
        }

        public async Task UpdateProfileAsync(CustomerUpdateProfileDto dto)
        {
            await contextFactory.WriteWithDbContextAsync(async context =>
            {
                var customer = await context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == authorizedCustomer.Id && c.IsActive)
                    .ConfigureAwait(false) ?? throw new ResourceNotFoundException("Customer not found.");

                customer.FirstName = dto.FirstName;
                customer.LastName = dto.LastName;
                customer.UpdatedAt = DateTime.UtcNow;

                if (dto.ProfilePhoto is { Length: > 0 })
                {
                    customer.ProfilePhotoUrl = await imageService.UpdateImageAsync(dto.ProfilePhoto, customer.ProfilePhotoUrl, ImageTypeEnum.Customer)
                        .ConfigureAwait(false);
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public async Task UpdatePasswordAsync(CustomerUpdatePasswordDto dto)
        {
            await contextFactory.WriteWithDbContextAsync(async context =>
            {
                var customer = await context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == authorizedCustomer.Id && c.IsActive)
                    .ConfigureAwait(false) ?? throw new ResourceNotFoundException("Customer not found.");

                var verificationResult = passwordHasher.VerifyHashedPassword(
                    customer, customer.PasswordHash, dto.OldPassword);

                if (verificationResult == PasswordVerificationResult.Failed)
                    throw new BusinessException("Incorrect old password.");

                customer.PasswordHash = passwordHasher.HashPassword(customer, dto.NewPassword);
                customer.UpdatedAt = DateTime.UtcNow;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}
