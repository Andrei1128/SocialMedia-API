using SocialMedia.Models.DTOs;

namespace SocialMedia.Repository.IRepository
{
    public interface IAuthRepository
    {
        bool IsEmailUnique(string email);
        Task<AuthResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<AuthResponseDTO> Register(RegisterRequestDTO registerRequestDTO);
    }
}
