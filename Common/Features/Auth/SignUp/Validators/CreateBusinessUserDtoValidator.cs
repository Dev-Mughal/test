using Common.Features.Auth.SignUp.DTOs;
using FluentValidation;

namespace Common.Features.Auth.SignUp.Validators
{
    public class CreateBusinessUserDtoValidator : AbstractValidator<CreateBusinessUserDto>
    {
        public CreateBusinessUserDtoValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty()
                .WithMessage("First Name is required.")
                .MaximumLength(50)
                .WithMessage("Maximum character length is 50.");

            RuleFor(u => u.LastName)
                .NotEmpty()
                .WithMessage("Last Name is required.")
                .MaximumLength(50)
                .WithMessage("Maximum character length is 50.");

            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Email must be a valid email address.");

            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters.");
        }
    }
}
