using SocialMedia.Models;

namespace SocialMedia.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<Post>> GetFeed(int id, int pageSize = 0, int pageNumber = 1);
        Task<User> UpdateAsync(User entity);
    }
}
