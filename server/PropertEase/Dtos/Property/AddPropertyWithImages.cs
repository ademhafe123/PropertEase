namespace PropertEase.Dtos.Property
{
    public class AddPropertyWithImages
    {
        public AddPropertyDto? Property { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
