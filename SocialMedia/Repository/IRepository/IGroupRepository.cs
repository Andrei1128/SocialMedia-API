using SocialMedia.Models;

namespace SocialMedia.Repository.IRepository
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<Group> UpdateAsync(Group entity);
    }
}
