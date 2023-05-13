using SocialMedia.Data;
using SocialMedia.Models;
using SocialMedia.Repository.IRepository;

namespace SocialMedia.Repository
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        private readonly ApplicationDbContext _db;

        public GroupRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Group> UpdateAsync(Group entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Groups.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
