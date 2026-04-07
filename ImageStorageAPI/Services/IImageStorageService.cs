
using Common.Models;

namespace ImageStorageAPI.Services
{
    public interface IImageStorageService
    {
        Task<string> UploadImageAsync(IFormFile imageFile, ImageTypeEnum imageType);
    }
}