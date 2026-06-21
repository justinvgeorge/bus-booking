using BusBooking.API.Models;
using BusBooking.API.Repositories.Interfaces;
using BusBooking.API.Services.Interfaces;
using Microsoft.AspNetCore.Routing;

namespace BusBooking.API.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IBusRouteRepository _busRouteRepository;

        public ScheduleService(IScheduleRepository scheduleRepository, IBusRouteRepository busRouteRepository)
        {
            _scheduleRepository = scheduleRepository;
            _busRouteRepository = busRouteRepository;
        }

        public async Task<IEnumerable<Schedule>> GetAllScheduleAsync()
        {
            return await _scheduleRepository.GetAllAsync();
        }
        public async Task<Schedule?> GetScheduleByIdAsync(int id)
        {
            return await _scheduleRepository.GetByIdAsync(id);
        }
        public async Task<PaginatedResult<Schedule>> SearchSchedulesAsync(string origin, string destination, DateTime? date, int page, int pageSize)
        {
            var busRoutes =  await _busRouteRepository.GetByOriginAndDestinationAsync(origin,destination);

            if (!busRoutes.Any()) 
            {
                return new PaginatedResult<Schedule>
                {
                    Items = new List<Schedule>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };
            }
            var schedules = new List<Schedule>();
            foreach(var busRoute in busRoutes)
            {
                var routeSchedule = await _scheduleRepository.GetByRouteAndDateAsync(busRoute.Id, date);
                
                schedules.AddRange(routeSchedule);
            }

            var totalCount = schedules.Count();
            var items = schedules.OrderBy(s => s.Departure)
                .Skip((page-1)*pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<Schedule>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

        }
        public async Task<Schedule> CreateScheduleAsync(Schedule schedule)
        {
            await _scheduleRepository.CreateAsync(schedule);
            return schedule;
        }
        public async Task<Schedule> UpdateScheduleAsync(Schedule schedule)
        {
            var schedulExist = await _scheduleRepository.GetByIdAsync(schedule.Id);
            if (schedulExist == null) { throw new Exception("This schedule does not exist"); }
            await _scheduleRepository.UpdateAsync(schedule);
            return schedule;
        }
        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var schedulExist = await _scheduleRepository.GetByIdAsync(id);
            if (schedulExist == null) { return false; }
            await _scheduleRepository.DeleteAsync(id);
            return true;

        }
    }
}
