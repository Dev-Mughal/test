using Common.Features.Business.DTOs;
using FluentValidation;

namespace Common.Features.Business.Validators
{
    public class UpdateBusinessDtoValidator : AbstractValidator<UpdateBusinessDto>
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB

        public UpdateBusinessDtoValidator()
        {
            RuleFor(b => b.BusinessName)
                .NotEmpty().WithMessage("Business Name is required.")
                .MaximumLength(150).WithMessage("Maximum character length is 150.");

            RuleFor(b => b.BusinessEmail)
                .NotEmpty().WithMessage("Business Email is required.")
                .EmailAddress().WithMessage("Business Email must be a valid email address.")
                .MaximumLength(100).WithMessage("Maximum character length is 100.");

            RuleFor(b => b.BusinessPhone)
                .Length(7, 15).WithMessage("Business Phone must be between 7 and 15 characters.")
                .NotEmpty().WithMessage("Business Phone is required.");

            RuleFor(b => b.CountryCode)
                .NotEmpty().WithMessage("Country Code is required.");

            RuleFor(b => b.BusinessURL)
                .NotEmpty().WithMessage("Business URL is required.")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var result)
                             && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps))
                .WithMessage("Business URL must be a valid HTTP or HTTPS URL.");

            RuleFor(b => b.StreetAddress).NotEmpty().WithMessage("Street Address is required.");
            RuleFor(b => b.City).NotEmpty().WithMessage("City is required.");
            RuleFor(b => b.State).NotEmpty().WithMessage("State is required.");
            RuleFor(b => b.ZipCode)
                .Length(4, 10).WithMessage("Zip Code must be between 4 and 10 characters.")
                .NotEmpty().WithMessage("Zip Code is required.");

            RuleFor(b => b.Country).NotEmpty().WithMessage("Country is required.");
            RuleFor(b => b.CategoryId).GreaterThan(0).WithMessage("Category is required.");
            RuleFor(b => b.Longitude).InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
            RuleFor(b => b.Latitude).InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(b => b.BusinessImage)
                .Must(file => file == null || file.Length <= MaxFileSizeInBytes)
                .WithMessage("Business image size must not exceed 5MB.")
                .Must(file => file == null || AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                .WithMessage("Business image must be a valid image file (jpg, jpeg, png, gif, webp).")
                .When(b => b.BusinessImage != null);
        }
    }
}

