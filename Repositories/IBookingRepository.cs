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

        Task <bool> HasRoomConflict(Booking booking);
        Task <bool> HasUserConflict(Booking booking);

        Task <bool> BookingExists(int id);
        Task<int> GetTodayBookingsCountAsync();
    }
}
