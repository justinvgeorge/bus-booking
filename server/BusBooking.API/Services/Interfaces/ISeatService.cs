using BusBooking.API.Models;

namespace BusBooking.API.Services.Interfaces
{
    public interface ISeatService
    {
        Task<IEnumerable<Seat>> GetSeatsByBusIdAsync(int busId);
        Task<bool> LockSeatAsync(int seatId, int userId);
        Task ReleaseLockAsync(int seatId);
        Task<bool> IsSeatLockedAsync(int seatId);
        Task<string?> GetSeatLockOwnerAsync(int seatId);
    }
}
