using AutoMapper;
using PropertEase.Dtos.User;

namespace PropertEase
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
        }
    }
}
