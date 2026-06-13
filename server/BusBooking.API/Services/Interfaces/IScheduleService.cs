using BusBooking.API.Models;

namespace BusBooking.API.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<IEnumerable<Schedule>> GetAllScheduleAsync();
        Task<Schedule?> GetScheduleByIdAsync(int id);
        Task<IEnumerable<Schedule>> SearchSchedulesAsync(string origin, string destination, DateTime date);
        Task<Schedule> CreateScheduleAsync(Schedule schedule);
        Task<Schedule> UpdateScheduleAsync(Schedule schedule);
        Task<bool> DeleteScheduleAsync(int id);
    }
}
