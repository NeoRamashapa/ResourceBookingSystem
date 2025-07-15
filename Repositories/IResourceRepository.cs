using InternalResourceBookingSystem.Models;

namespace InternalResourceBookingSystem.Repositories
{
    public interface IResourceRepository
    {
        Task<List<Resource>> GetAllAsync();
        Task<Resource> GetByIdAsync(int id);
        Task<Resource> GetByIdIncludeBookingAsync(int id);
        Task AddAsync(Resource resource);
        Task UpdateAsync(Resource resource);
        Task DeleteAsync(int id);
    }
}
