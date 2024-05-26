
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PropertEase.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        public AuthRepository(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }
        //Login an existing user
        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user is null)
            {
                response.Success = false;
                response.Message = "User does not exist";
                return response;
            }
            //VerifyPassword(user, password);
            //if true create token
            //else response.message="Incorrect password"
            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Incorrect password";
                return response;
            }

            var token = CreateToken(user);
            response.Data = token;
            response.Message = "Login successful";

            return response;
        }

        //Register a new user
        public async Task<ServiceResponse<string>> Register(User user, string password)
        {
            var response = new ServiceResponse<string>();
            if (await UserExists(user.Email))
            {
                response.Success = false;
                response.Message = "User with this email already exists";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            //Generate token
            var token = CreateToken(user);
            response.Data = token;
            response.Message = "User registered successfully";

            return response;
        }

        //Check if User exists
        public async Task<bool> UserExists(string email)
        {
            if (await _dataContext.Users.AnyAsync(u => u.Email == email))
            {
                return true;
            }
            return false;
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),

                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
