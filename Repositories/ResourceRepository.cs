using InternalResourceBookingSystem.Data;
using InternalResourceBookingSystem.Models;
using InternalResourceBookingSystem.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InternalResourceBookingSystem.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ApplicationDbContext _context;
        public ResourceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Resource>> GetAllAsync()
        {
            return await _context.Resources.ToListAsync();
        }

        public async Task<Resource> GetByIdAsync(int id)
        {
            return await _context.Resources.Include(r => r.Bookings).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Resource resource)
        {
            _context.Resources.Update(resource);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource != null)
            {
                _context.Resources.Remove(resource);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Resource> GetByIdIncludeBookingAsync(int id)
        {
            var resource = await _context.Resources
                 .Include(r => r.Bookings)
                 .FirstOrDefaultAsync(m => m.Id == id);

            return resource;
        }
        public async Task<Booking> GetByIdAsync()
        {
            return await _context.Bookings.FirstOrDefaultAsync();
        }
    }
}
