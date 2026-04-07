using Common.Features.Auth.SignUp.DTOs;
using FluentValidation;

namespace Common.Features.Auth.SignUp.Validators
{
    public class SignUpDtoValidator : AbstractValidator<SignUpDto>
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB

        public SignUpDtoValidator()
        {
            #region BUSINESS VALIDATION
            RuleFor(s => s.BusinessName)
                .NotEmpty().WithMessage("Business Name is required.")
                .MaximumLength(150).WithMessage("Business Name maximum length is 150 characters.");

            RuleFor(s => s.BusinessEmail)
                .EmailAddress().WithMessage("Business Email must be a valid email address.")
                .MaximumLength(100).WithMessage("Business Email maximum length is 100 characters.")
                .When(s => !string.IsNullOrWhiteSpace(s.BusinessEmail));

            RuleFor(s => s.BusinessPhone)
                .NotEmpty().WithMessage("Business Phone is required.")
                .Length(7, 15).WithMessage("Business Phone must be between 7 and 15 characters.");

            RuleFor(s => s.CountryCode)
                .NotEmpty().WithMessage("Country Code is required.");

            RuleFor(s => s.BusinessURL)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var result)
                             && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps))
                .WithMessage("Business URL must be a valid HTTP or HTTPS URL.")
                .When(s => !string.IsNullOrWhiteSpace(s.BusinessURL));

            RuleFor(s => s.StreetAddress)
                .NotEmpty().WithMessage("Street Address is required.");

            RuleFor(s => s.AddressLine2)
                .MaximumLength(150).WithMessage("Address Line 2 maximum length is 150 characters.")
                .When(s => !string.IsNullOrWhiteSpace(s.AddressLine2));

            RuleFor(s => s.City)
                .NotEmpty().WithMessage("City is required.");

            RuleFor(s => s.State)
                .NotEmpty().WithMessage("State is required.")
                .Length(2).WithMessage("State must be a valid 2-character US state code (e.g., CA, TX, NY).")
                .Matches(@"^[A-Z]{2}$").WithMessage("State must be exactly 2 uppercase letters (e.g., CA, TX, NY).");

            RuleFor(s => s.ZipCode)
                .NotEmpty().WithMessage("Zip Code is required.")
                .Length(4, 10).WithMessage("Zip Code must be between 4 and 10 characters.");

            RuleFor(s => s.Country)
                .NotEmpty().WithMessage("Country is required.");

            RuleFor(s => s.CategoryId)
                .GreaterThan(0).WithMessage("A valid Category is required.");

            RuleFor(s => s.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");

            RuleFor(s => s.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(s => s.BusinessImage)
                .Must(file => file == null || file.Length <= MaxFileSizeInBytes)
                .WithMessage("Business image size must not exceed 5MB.")
                .Must(file => file == null || AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                .WithMessage("Business image must be a valid image file (jpg, jpeg, png, gif, webp).")
                .When(s => s.BusinessImage != null);
            #endregion

            #region USER VALIDATION
            RuleFor(s => s.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(50).WithMessage("First Name maximum length is 50 characters.");

            RuleFor(s => s.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(50).WithMessage("Last Name maximum length is 50 characters.");

            RuleFor(s => s.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.")
                .MaximumLength(100).WithMessage("Email maximum length is 100 characters.");

            RuleFor(s => s.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.");

            RuleFor(s => s.TimeZone)
                .NotEmpty().WithMessage("TimeZone is required.")
                .MaximumLength(100).WithMessage("TimeZone must not exceed 100 characters.");
            #endregion
        }
    }
}
