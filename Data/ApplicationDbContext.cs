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

        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {//1:M relationship between Resource and Booking
            base.OnModelCreating(modelBuilder);
            // Configure Resource entity
            modelBuilder.Entity<Booking>()
               .HasOne(b => b.Resource)
               .WithMany(r => r.Bookings)
               .HasForeignKey(b => b.ResourceId)
               .OnDelete(DeleteBehavior.Cascade);



        }


    }
}
