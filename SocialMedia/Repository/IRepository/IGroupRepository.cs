using FirstProject_API.Models;

namespace FirstProject_API.Repository.IRepository
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<Group> UpdateAsync(Group entity);
    }
}
