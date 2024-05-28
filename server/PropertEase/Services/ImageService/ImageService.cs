
namespace PropertEase.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly string? _imageStoragePath;
        public ImageService(IConfiguration configuration)
        {
            _imageStoragePath = configuration["ImageStorage:Path"];
            if (!Directory.Exists(_imageStoragePath))
            {
                Directory.CreateDirectory(_imageStoragePath);
            }
        }
        public string GetImagePath(string fileName)
        {
            return Path.Combine(_imageStoragePath, fileName);

        }

        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Invalid image file");

            var fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(_imageStoragePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return fileName;
        }

        public async Task<List<string>> SaveImagesAsync(List<IFormFile> images)
        {
            var imageFilePaths = new List<string>();

            foreach (var image in images)
            {
                var filePath = await SaveImageAsync(image); // Implement SaveImageAsync to handle the actual saving
                if (!string.IsNullOrEmpty(filePath))
                {
                    imageFilePaths.Add(filePath);
                }
            }

            return imageFilePaths;
        }
    }
}
