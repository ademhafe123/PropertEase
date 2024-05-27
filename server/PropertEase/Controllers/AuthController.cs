using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertEase.Dtos.User;
using PropertEase.Services.UserService;
using System.Security.Claims;

namespace PropertEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserService _userService;
        public AuthController(IAuthRepository authRepository, IUserService service)
        {
            _authRepository = authRepository;
            _userService = service;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<string>>> Register(RegisterUserDto user)
        {

            var response = await _authRepository.Register(new User
            {
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = DateTime.Today,
            }, user.Password);

            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(LoginUserDto user)
        {
            var response = await _authRepository.Login(user.Email, user.Password);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        [HttpGet("GetUser")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetUser()
        {
            var claimUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimUserId) || !int.TryParse(claimUserId, out int userId))
            {
                return BadRequest("User ID not found in claims or invalid.");
            }

            var response = await _userService.GetUserById(userId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);


        }
    }
}
