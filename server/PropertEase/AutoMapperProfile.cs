using AutoMapper;
using PropertEase.Dtos.Property;
using PropertEase.Dtos.User;

namespace PropertEase
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
            CreateMap<GetUserDto, User>();
            CreateMap<Property, AddPropertyDto>();
            CreateMap<AddPropertyDto, Property>();
            CreateMap<GetPropertyDto, Property>();
            CreateMap<Property, GetPropertyDto>();
        }
    }
}
