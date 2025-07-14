using InternalResourceBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalResourceBookingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Resource> Resources { get; set; }
    }
}
