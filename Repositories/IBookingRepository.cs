using InternalResourceBookingSystem.Models;

namespace InternalResourceBookingSystem.Repositories
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllAsync();
        Task<Booking> GetByIdAsync(int id);
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
        Task <bool> CheckingBookingConflict(Booking booking);
        Task <bool> CheckingUserBookingConflict(Booking booking);
        Task <bool> BookingExists(int id);
    }
}
