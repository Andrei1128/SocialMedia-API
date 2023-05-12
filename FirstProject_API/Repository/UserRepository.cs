using FirstProject_API.Data;
using FirstProject_API.Models;
using FirstProject_API.Models.DTOs;
using FirstProject_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FirstProject_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public bool IsEmailUnique(string email)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return true;
            else return false;
        }

        private string GenerateJWT(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                }
                ),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == loginRequestDTO.Email);
            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }
            var token = GenerateJWT(user);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = token,
                User = user
            };
            return loginResponseDTO;
        }

        public async Task<LoginResponseDTO> Register(RegisterRequestDTO registerRequestDTO)
        {
            User user = new User()
            {
                Email = registerRequestDTO.Email,
                Name = registerRequestDTO.Name,
                Password = registerRequestDTO.Password,
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            var token = GenerateJWT(user);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = token,
                User = user
            };
            return loginResponseDTO;
        }
    }
}
