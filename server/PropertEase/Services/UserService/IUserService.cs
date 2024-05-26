using PropertEase.Dtos.User;

namespace PropertEase.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<GetUserDto>> GetUserById(int id);
    }
}
