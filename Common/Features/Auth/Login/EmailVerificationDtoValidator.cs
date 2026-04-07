using FluentValidation;

namespace Common.Features.Auth.Login
{
    public class EmailVerificationDtoValidator : AbstractValidator<EmailVerificationDto>
    {
        public EmailVerificationDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");
        }
    }
}
