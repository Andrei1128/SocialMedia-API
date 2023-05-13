using FirstProject_API.Data;
using FirstProject_API.Models;
using FirstProject_API.Repository.IRepository;

namespace FirstProject_API.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _db;

        public PostRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Post> UpdateAsync(Post entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Posts.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
