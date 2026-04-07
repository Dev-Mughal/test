using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using Application.Services.Interfaces;

namespace Application.Services.Implementations
{
    public class ImageService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ImageService> logger) : IImageService
    {
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB
        private const string ImageServerBaseUrlKey = "ImageServer:BaseUrl";
        private const string ImageServerUploadEndpointKey = "ImageServer:UploadEndpoint";
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".svg"];
        private static readonly string[] AllowedMimeTypes = 
        [
            "image/jpeg", 
            "image/png", 
            "image/gif", 
            "image/webp", 
            "image/bmp", 
            "image/svg+xml"
        ];

        public async Task<string> SaveImageAsync(IFormFile file, ImageTypeEnum imageType)
        {
            ArgumentNullException.ThrowIfNull(file);

            if (!IsValidImage(file))
            {
                throw new ArgumentException("Invalid image file. Please upload a valid image (jpg, jpeg, png, gif, webp, bmp, svg) under 5MB.");
            }

            var uploadUrl = BuildUploadUrl(imageType);

            try
            {
                using var content = new MultipartFormDataContent();
                await using var stream = file.OpenReadStream();
                using var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(streamContent, "file", file.FileName);

                using var response = await httpClient.PostAsync(uploadUrl, content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    logger.LogError("Image upload failed. Status: {StatusCode}, Body: {Body}", response.StatusCode, errorBody);
                    throw new InvalidOperationException("Failed to upload image to image server.");
                }

                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var relativePath = ExtractPath(responseBody);

                if (string.IsNullOrWhiteSpace(relativePath))
                    throw new InvalidOperationException("Image server returned an empty path.");

                logger.LogInformation("Image uploaded successfully: {ImageUrl}", relativePath);
                
                return relativePath;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to upload image to image server.");
                throw new InvalidOperationException("Failed to save image. Please try again.", ex);
            }
        }

        public async Task<string> UpdateImageAsync(IFormFile file, string? existingImageUrl, ImageTypeEnum imageType)
        {
            // Delete existing image if it exists
            if (!string.IsNullOrWhiteSpace(existingImageUrl))
            {
                await DeleteImageAsync(existingImageUrl).ConfigureAwait(false);
            }

            // Save the new image
            return await SaveImageAsync(file, imageType).ConfigureAwait(false);
        }

        public Task<bool> DeleteImageAsync(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Task.FromResult(false);
            }

            // Image deletion is managed by the external image storage service.
            // This API currently persists only relative paths.
            logger.LogWarning("DeleteImageAsync is not implemented for remote image storage. Path: {ImageUrl}", imageUrl);
            return Task.FromResult(false);
        }

        public async Task<int> DeleteImagesAsync(IEnumerable<string?> imageUrls)
        {
            var deletedCount = 0;

            foreach (var imageUrl in imageUrls)
            {
                if (await DeleteImageAsync(imageUrl).ConfigureAwait(false))
                {
                    deletedCount++;
                }
            }

            logger.LogInformation("Deleted {Count} images", deletedCount);
            return deletedCount;
        }

        public bool IsValidImage(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            // Check file size
            if (file.Length > MaxFileSizeInBytes)
            {
                return false;
            }

            // Check file extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                return false;
            }

            // Check MIME type
            if (!AllowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            {
                return false;
            }

            return true;
        }

        public string GetFileSizeDisplay(IFormFile file)
        {
            ArgumentNullException.ThrowIfNull(file);

            var sizeInBytes = file.Length;

            return sizeInBytes switch
            {
                < 1024 => $"{sizeInBytes} B",
                < 1024 * 1024 => $"{sizeInBytes / 1024.0:F2} KB",
                _ => $"{sizeInBytes / (1024.0 * 1024.0):F2} MB"
            };
        }

        #region Private Helper Methods

        public string? GetPublicImageUrl(string? relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
                return null;

            if (Uri.TryCreate(relativeUrl, UriKind.Absolute, out _))
                return relativeUrl;

            var baseUrl = configuration[ImageServerBaseUrlKey];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                logger.LogWarning("ImageServer:BaseUrl is not configured. Returning relative image path as-is.");
                return relativeUrl;
            }

            return $"{baseUrl.TrimEnd('/')}/{relativeUrl.TrimStart('/')}";
        }

        private string BuildUploadUrl(ImageTypeEnum imageType)
        {
            var baseUrl = configuration[ImageServerBaseUrlKey];
            var uploadEndpoint = configuration[ImageServerUploadEndpointKey];

            if (string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(uploadEndpoint))
                throw new InvalidOperationException("ImageServer configuration is missing BaseUrl or UploadEndpoint.");

            return $"{baseUrl.TrimEnd('/')}/{uploadEndpoint.Trim('/')}/{(int)imageType}";
        }

        private static string ExtractPath(string responseBody)
        {
            if (string.IsNullOrWhiteSpace(responseBody))
                return string.Empty;

            responseBody = responseBody.Trim();

            if (responseBody.StartsWith('"') && responseBody.EndsWith('"'))
            {
                return JsonSerializer.Deserialize<string>(responseBody) ?? string.Empty;
            }

            return responseBody;
        }

        #endregion
    }
}
