using PropertEase.Dtos.Property;

namespace PropertEase.Services.PropertyService
{
    public interface IPropertyService
    {
        Task<ServiceResponse<string>> AddProperty(AddPropertyDto newProperty, int userId, List<IFormFile> images);
        Task<ServiceResponse<GetPropertyDto>> GetProperty(int propertyId, int userId);
        Task<ServiceResponse<List<GetPropertyDto>>> GetAllProperties();
        Task<ServiceResponse<List<GetPropertyDto>>> GetUserProperties(int userId);
        Task<ServiceResponse<string>> DeleteProperty(int propertyId, int userId);
    }
}
