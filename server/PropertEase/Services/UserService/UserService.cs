using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PropertEase.Dtos.User;

namespace PropertEase.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public UserService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetUserDto>> GetUserById(int id)
        {
            var response = new ServiceResponse<GetUserDto>();

            try
            {
                var foundUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (foundUser is null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }
                response.Data = _mapper.Map<GetUserDto>(foundUser);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
