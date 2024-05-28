namespace PropertEase.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public int PropertyId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public Property? Property { get; set; }

    }
}
