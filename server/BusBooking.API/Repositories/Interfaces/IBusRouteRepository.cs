using BusBooking.API.Models;

namespace BusBooking.API.Repositories.Interfaces
{
    public interface IBusRouteRepository
    {
        Task<IEnumerable<BusRoute>> GetAllAsync();
        Task<BusRoute?> GetByIdAsync(int id);
        Task<IEnumerable<BusRoute>> GetByOriginAndDestinationAsync(string origin, string destination);
        Task<BusRoute> CreateAsync(BusRoute route);
        Task<BusRoute> UpdateAsync(BusRoute route);
        Task<bool> DeleteAsync(int id);

    }
}
