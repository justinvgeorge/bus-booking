using BusBooking.API.Models;

namespace BusBooking.API.Repositories.Interfaces
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>> GetAllAsync();
        Task<Schedule?> GetByIdAsync(int id);
        Task<IEnumerable<Schedule>> GetByRouteAndDateAsync(int routeId, DateTime? date);
        Task<Schedule> CreateAsync(Schedule schedule);
        Task<Schedule> UpdateAsync(Schedule schedule);
        Task<bool> DeleteAsync(int id);
    }
}
