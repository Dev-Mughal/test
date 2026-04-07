using Common.Features.Coupon.DTOs;
using FluentValidation;

namespace Common.Features.Coupon.Validators
{
    public class CreateCouponDtoValidator : AbstractValidator<CreateCouponDto>
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB

        public CreateCouponDtoValidator()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(150).WithMessage("Title must not exceed 150 characters.");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(c => c.StartDateTime)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(c => c.EndDateTime)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThan(c => c.StartDateTime).WithMessage("End date must be after start date.");

            RuleFor(c => c.ExpirationTime)
                .GreaterThanOrEqualTo(c => c.EndDateTime).WithMessage("Expiration time must be on or after the end date.")
                .When(c => c.ExpirationTime.HasValue);

            RuleFor(c => c.Photo)
                .Must(file => file == null || file.Length <= MaxFileSizeInBytes)
                .WithMessage("Photo size must not exceed 5MB.")
                .Must(file => file == null || AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                .WithMessage("Photo must be a valid image file (jpg, jpeg, png, gif, webp).")
                .When(c => c.Photo != null);
        }
    }
}


