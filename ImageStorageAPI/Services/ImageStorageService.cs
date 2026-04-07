using Common.Models;
using Microsoft.AspNetCore.Hosting;

namespace ImageStorageAPI.Services
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly IWebHostEnvironment _env;

        public ImageStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, ImageTypeEnum imageType)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Image file Or Path cannot be null or empty.");

            if (!IsImageFile(imageFile.FileName))
                return string.Empty;

            var folder = ResolveFolder(imageType);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";

            var wwwRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var folderPath = Path.Combine(wwwRoot, folder);
            Directory.CreateDirectory(folderPath);

            var imagePath = Path.Combine(folderPath, fileName);
            await using var stream = new FileStream(imagePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await imageFile.CopyToAsync(stream).ConfigureAwait(false);

            return $"/{folder}/{fileName}";
        }

        private static bool IsImageFile(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension is ".jpg" or ".jpeg" or ".png" or ".gif" or ".webp" or ".bmp" or ".svg";
        }

        // Keep current storage structure untouched.
        private static string ResolveFolder(ImageTypeEnum imageType) => imageType switch
        {
            ImageTypeEnum.Business => "BusinessLogos",
            ImageTypeEnum.Coupon => "CouponImages",
            ImageTypeEnum.Customer => "CustomerImages",
            ImageTypeEnum.Incentive => "IncentiveImages",
            _ => throw new ArgumentOutOfRangeException(nameof(imageType), imageType, "Unsupported image type.")
        };

    }
}
