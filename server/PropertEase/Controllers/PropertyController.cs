using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertEase.Dtos.Property;
using PropertEase.Services.PropertyService;
using PropertEase.Services.UserService;
using System.Security.Claims;

namespace PropertEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IPropertyService _propertyService;
        public PropertyController(IUserService userService, IMapper mapper, IPropertyService propertyService)
        {
            _userService = userService;
            _mapper = mapper;
            _propertyService = propertyService;
        }
        //ADD PROPERTY
        [HttpPost("AddProperty")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<string>>> AddProperty([FromForm] AddPropertyWithImages newPropertyWithImages)
        {
            var claimUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claimUserId) || !int.TryParse(claimUserId, out int userId))
            {
                return BadRequest("User ID not found in claims or invalid.");
            }

            var response = await _propertyService.AddProperty(newPropertyWithImages.Property, userId, newPropertyWithImages.Images);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetProperty")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<GetPropertyDto>>> GetProperty(int propertyId)
        {
            var claimUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claimUserId) || !int.TryParse(claimUserId, out int userId))
            {
                return BadRequest("User ID not found in claims or invalid.");
            }

            var response = await _propertyService.GetProperty(propertyId, userId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllProperties")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<List<GetPropertyDto>>>> GetAllProperties()
        {
            var response = await _propertyService.GetAllProperties();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetUserProperties")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<List<GetPropertyDto>>>> GetAllUserProperties()
        {
            var claimsUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (claimsUserId == null || !int.TryParse(claimsUserId, out int userId))
            {
                return BadRequest("User ID not found in claims or invalid.");
            }
            var response = await _propertyService.GetUserProperties(userId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteProperty")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteProperty(int propertyId)
        {
            var claimsUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (claimsUserId == null || !int.TryParse(claimsUserId, out int userId))
            {
                return BadRequest("User ID not found in claims or invalid.");
            }
            var response = await _propertyService.DeleteProperty(propertyId, userId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
