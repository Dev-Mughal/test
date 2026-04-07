using Common.Features.Customer.Auth.DTOs;
using FluentValidation;

namespace Common.Features.Customer.Auth.Validators
{
    public class CustomerSignUpDtoValidator : AbstractValidator<CustomerSignUpDto>
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB
        public CustomerSignUpDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.");

            RuleFor(x => x.TimeZone)
                .NotEmpty().WithMessage("Timezone is required.");
            RuleFor(b => b.ProfilePhoto)
                .Must(file => file == null || file.Length <= MaxFileSizeInBytes)
                .WithMessage("Business image size must not exceed 5MB.")
                .Must(file => file == null || AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                .WithMessage("Business image must be a valid image file (jpg, jpeg, png, gif, webp).")
                .When(b => b.ProfilePhoto != null);
        }
    }
}
