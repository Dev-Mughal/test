using Microsoft.AspNetCore.Http;
using Common.Models;

namespace Application.Services.Interfaces
{
    public interface IImageService
    {
        /// <summary>
        /// Saves an image to the specified folder.
        /// </summary>
        /// <param name="file">The image file to save.</param>
        /// <param name="imageType">Logical image type used to resolve the configured folder path.</param>
        /// <returns>The relative URL path of the saved image.</returns>
        Task<string> SaveImageAsync(IFormFile file, ImageTypeEnum imageType);

        /// <summary>
        /// Updates an existing image by deleting the old one and saving the new one.
        /// </summary>
        /// <param name="file">The new image file to save.</param>
        /// <param name="existingImageUrl">The URL of the existing image to delete (can be null).</param>
        /// <param name="imageType">Logical image type used to resolve the configured folder path.</param>
        /// <returns>The relative URL path of the new saved image.</returns>
        Task<string> UpdateImageAsync(IFormFile file, string? existingImageUrl, ImageTypeEnum imageType);

        /// <summary>
        /// Deletes an image from the file system.
        /// </summary>
        /// <param name="imageUrl">The relative URL of the image to delete.</param>
        /// <returns>True if the image was deleted successfully, false otherwise.</returns>
        Task<bool> DeleteImageAsync(string? imageUrl);

        /// <summary>
        /// Deletes multiple images from the file system.
        /// </summary>
        /// <param name="imageUrls">The collection of image URLs to delete.</param>
        /// <returns>The number of images successfully deleted.</returns>
        Task<int> DeleteImagesAsync(IEnumerable<string?> imageUrls);

        /// <summary>
        /// Validates if the file is a valid image.
        /// </summary>
        /// <param name="file">The file to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        bool IsValidImage(IFormFile? file);

        /// <summary>
        /// Gets the file size in a human-readable format.
        /// </summary>
        /// <param name="file">The file to get size for.</param>
        /// <returns>Human-readable file size string.</returns>
        string GetFileSizeDisplay(IFormFile file);

        /// <summary>
        /// Builds the full public URL for an image using the configured image server base URL.
        /// Returns null if relativeUrl is null or empty.
        /// </summary>
        string? GetPublicImageUrl(string? relativeUrl);
    }
}
