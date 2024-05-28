namespace PropertEase.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Location { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string PropertyType { get; set; } = string.Empty;
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Area { get; set; }
        public string HomeAppliances { get; set; } = string.Empty;
        public ICollection<Image>? Images { get; set; } = new List<Image>();
        public User? ListedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Today;

    }
}
