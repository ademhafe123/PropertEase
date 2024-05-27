namespace PropertEase.Dtos.Property
{
    public class GetPropertyDto
    {
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
        public int ListedById { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
