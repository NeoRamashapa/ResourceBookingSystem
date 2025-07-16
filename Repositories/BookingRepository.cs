using Elfie.Serialization;
using Humanizer.Localisation;
using InternalResourceBookingSystem.Data;
using InternalResourceBookingSystem.Models;
using InternalResourceBookingSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InternalResourceBookingSystem.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasRoomConflict( Booking booking)
        {
            // Here I am checking if the booking overlaps with any existing bookings for the same meeting room
            bool bookingConflict = await _context.Bookings.AnyAsync(b =>
                b.ResourceId == booking.ResourceId &&
                b.Id != booking.Id &&
                b.StartTime < booking.EndTime &&
                booking.StartTime < b.EndTime);
            return bookingConflict;
        }
        public async Task<bool> HasUserConflict( Booking booking) 
            {
            // Here I am checking if the user has another booking during the same time for a different room
            bool userBookingConflict = await _context.Bookings.AnyAsync(b =>
                b.BookedBy == booking.BookedBy &&
                b.Id != booking.Id &&
                b.StartTime < booking.EndTime &&
                booking.StartTime < b.EndTime);

            return userBookingConflict;
            }
        public async Task<bool> BookingExists(int id)
        {
            return await _context.Bookings.AnyAsync(e => e.Id == id);
        }
        public async Task DeleteAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings.Include(b => b.Resource).OrderBy(b => b.StartTime).ToListAsync();
        }

        public async Task<int> GetTodayBookingsCountAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _context.Bookings
                .Where(b => b.StartTime >= today && b.StartTime < tomorrow)
                .CountAsync();
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _context.Bookings.Include(b => b.Resource).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
    }
}
