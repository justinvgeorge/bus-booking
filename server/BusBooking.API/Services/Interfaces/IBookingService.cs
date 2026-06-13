using BusBooking.API.Models;

namespace BusBooking.API.Services.Interfaces
{
    public interface IBookingService
    {
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId);
        Task<Booking> CreateBookingAsync(int userId, int scheduleId, int seatId);
        Task<bool> CancelBookingAsync(int bookingId, int userId);
    }
}
