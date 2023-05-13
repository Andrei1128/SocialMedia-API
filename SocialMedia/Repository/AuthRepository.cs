using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Data;
using SocialMedia.Models;
using SocialMedia.Models.DTOs;
using SocialMedia.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SocialMedia.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        private readonly IMapper _mapper;
        internal DbSet<User> _dbSet;

        public AuthRepository(ApplicationDbContext db, IConfiguration configuration, IMapper mapper)
        {
            this._dbSet = db.Set<User>();
            _mapper = mapper;
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
                    new Claim("MyUserId", user.Id.ToString()),
                }
                ),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string EncodePassword(string password)
        {
            string base64HashedPasswordBytes;
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                base64HashedPasswordBytes = Convert.ToBase64String(hashedPasswordBytes);
            }
            return base64HashedPasswordBytes;
        }

        public async Task<AuthResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            IQueryable<User> query = _dbSet;
            query = query.Where(u => u.Email == loginRequestDTO.Email && u.Password == EncodePassword(loginRequestDTO.Password));
            foreach (var includeProp in "Requests,Friends,Groups,Posts".Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp);

            User user = await query.FirstOrDefaultAsync();
            if (user == null)
            {
                return new AuthResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }
            var token = GenerateJWT(user);
            AuthResponseDTO loginResponseDTO = new AuthResponseDTO()
            {
                Token = token,
                User = _mapper.Map<UserDTO>(user)
            };
            return loginResponseDTO;
        }

        public async Task<AuthResponseDTO> Register(RegisterRequestDTO registerRequestDTO)
        {
            User user = new User()
            {
                Email = registerRequestDTO.Email,
                Name = registerRequestDTO.Name,
                Password = EncodePassword(registerRequestDTO.Password),
                Friends = new List<UserFriend>(),
                Groups = new List<Group>(),
                Posts = new List<Post>(),
                Requests = new List<UserRequest>()
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            var token = GenerateJWT(user);
            AuthResponseDTO loginResponseDTO = new AuthResponseDTO()
            {
                Token = token,
                User = _mapper.Map<UserDTO>(user)
            };
            return loginResponseDTO;
        }
    }
}
