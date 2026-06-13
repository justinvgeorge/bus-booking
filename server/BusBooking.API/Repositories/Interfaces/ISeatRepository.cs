using BusBooking.API.Models;

namespace BusBooking.API.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        Task<Seat?> GetByIdAsync(int id);
        Task<IEnumerable<Seat>> GetByBusIdAsync(int busId);
        Task<IEnumerable<Seat>> GetAvailableSeatsAsync(int busId);
        Task <Seat> CreateAsync(Seat seat);
        Task<Seat> UpdateAsync(Seat seat);


    }
}
