using Common.Features.Customer.Auth.DTOs;
using FluentValidation;

namespace Common.Features.Customer.Auth.Validators
{
    public class CustomerUpdatePasswordDtoValidator : AbstractValidator<CustomerUpdatePasswordDto>
    {
        public CustomerUpdatePasswordDtoValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Old password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Please confirm your new password.")
                .Equal(x => x.NewPassword).WithMessage("The new password and confirmation password do not match.");
        }
    }
}
