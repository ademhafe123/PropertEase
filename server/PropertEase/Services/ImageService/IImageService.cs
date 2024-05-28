namespace PropertEase.Services.ImageService
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile);
        string GetImagePath(string fileName);
        Task<List<string>> SaveImagesAsync(List<IFormFile> images);
    }
}
