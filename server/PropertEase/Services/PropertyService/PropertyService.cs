using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PropertEase.Dtos.Property;
using PropertEase.Services.ImageService;
using PropertEase.Services.UserService;

namespace PropertEase.Services.PropertyService
{
    public class PropertyService : IPropertyService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;
        public PropertyService(DataContext dataContext, IMapper mapper, IUserService userService, IImageService imageService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _userService = userService;
            _imageService = imageService;
        }
        public async Task<ServiceResponse<string>> AddProperty(AddPropertyDto newProperty, int userId, List<IFormFile> images)
        {
            var response = new ServiceResponse<string>();
            if (newProperty is null)
            {
                response.Success = false;
                response.Message = "New property is not defined";
                return response;
            }
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }
            Property property = _mapper.Map<Property>(newProperty);
            property.ListedBy = user;
            //Make a call to _imageService and forward the images
            //Add the images to the Images field of the property object
            var imageFilePaths = await _imageService.SaveImagesAsync(images);
            foreach (var filePath in imageFilePaths)
            {
                property.Images?.Add(new Image
                {
                    FilePath = filePath,
                    Property = property
                });
            }

            _dataContext.Properties.Add(property);
            await _dataContext.SaveChangesAsync();

            response.Message = "Property successfully added";
            response.Data = property.Id.ToString();

            return response;
        }

        public async Task<ServiceResponse<GetPropertyDto>> GetProperty(int propertyId, int userId)
        {
            var response = new ServiceResponse<GetPropertyDto>();

            try
            {
                var foundProperty = await _dataContext.Properties
                    .Include(p => p.Images)  // Include the Images
                    .Include(p => p.ListedBy)
                    .FirstOrDefaultAsync(p => p.Id == propertyId);
                if (foundProperty is null)
                {
                    response.Success = false;
                    response.Message = "Property not found";
                    return response;
                }

                GetPropertyDto property = _mapper.Map<GetPropertyDto>(foundProperty);
                property.ImagePaths = foundProperty.Images.Select(i => i.FilePath).ToList();
                property.ListedById = userId;

                response.Data = property;
                response.Message = "Property found successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }


        public async Task<ServiceResponse<List<GetPropertyDto>>> GetAllProperties()
        {
            var response = new ServiceResponse<List<GetPropertyDto>>();

            try
            {
                var dbProperties = await _dataContext.Properties
                    .Include(p => p.Images)
                    .Include(p => p.ListedBy).ToListAsync();

                if (dbProperties is null)
                {
                    response.Success = false;
                    response.Message = "No properties found";
                    return response;
                }
                List<GetPropertyDto> properties = dbProperties.Select(p =>
                {
                    var propertyDto = _mapper.Map<GetPropertyDto>(p);
                    if (p.ListedBy != null)
                    {
                        propertyDto.ListedById = p.ListedBy.Id;
                        propertyDto.ImagePaths = p.Images.Select(i => i.FilePath).ToList();
                    }
                    return propertyDto;
                }).ToList();
                response.Data = properties;
                response.Message = "All properties found successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetPropertyDto>>> GetUserProperties(int userId)
        {
            var response = new ServiceResponse<List<GetPropertyDto>>();
            try
            {
                var userPropertiesDb = await _dataContext.Properties
                    .Where(p => p.ListedBy.Id == userId)
                    .Include(p => p.Images)
                    .Include(p => p.ListedBy) //This is just to make sure that this is loaded (Good practice)
                    .ToListAsync();
                if (userPropertiesDb is null)
                {
                    response.Message = "User has no listed properties";
                    return response;
                }
                List<GetPropertyDto> userProperties = userPropertiesDb.Select(p =>
                {
                    var propertyDto = _mapper.Map<GetPropertyDto>(p);
                    if (p.ListedBy != null)
                    {
                        propertyDto.ListedById = p.ListedBy.Id;
                        propertyDto.ImagePaths = p.Images.Select(i => i.FilePath).ToList();
                    }
                    return propertyDto;
                }).ToList();

                response.Data = userProperties;
                response.Message = "User properties retrieved successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<ServiceResponse<string>> DeleteProperty(int propertyId, int userId)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var property = await _dataContext.Properties
                    .Include(p => p.ListedBy)
                    .FirstOrDefaultAsync(p => p.Id == propertyId);
                if (property is null)
                {
                    response.Success = false;
                    response.Message = "Property not found";
                    return response;
                }

                //Check if user that with Id: userId is the one that created a listing
                //for this property, with Id: propertyId, and only if it is, it should be able to delete the property
                if (property.ListedBy == null || property.ListedBy.Id != userId)
                {
                    response.Success = false;
                    response.Message = "User is not authorized to delete this property";
                    return response;
                }


                _dataContext.Properties.Remove(property);
                await _dataContext.SaveChangesAsync();

                response.Message = "Property deleted successfully";
                response.Data = "Property deleted";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }



    }
}
