using FirstProject_API.Models.DTOs;

namespace FirstProject_API.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsEmailUnique(string email);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LoginResponseDTO> Register(RegisterRequestDTO registerRequestDTO);
    }
}
