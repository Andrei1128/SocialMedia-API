using FirstProject_API.Models;

namespace FirstProject_API.Repository.IRepository
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post> UpdateAsync(Post entity);
    }
}
