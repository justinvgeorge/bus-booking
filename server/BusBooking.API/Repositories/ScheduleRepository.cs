using BusBooking.API.Data;
using BusBooking.API.Models;
using BusBooking.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BusBooking.API.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly AppDbContext _context;
        public ScheduleRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            return await _context.Schedules.Include(x=>x.Bus).Include(x=>x.Route).ToListAsync();
        }
        public async Task<Schedule?> GetByIdAsync(int id)
        {
            return await _context.Schedules.Include(x => x.Bus).Include(x => x.Route).FirstOrDefaultAsync(x=>x.Id == id);
        }
        public async Task<IEnumerable<Schedule>> GetByRouteAndDateAsync(int routeId, DateTime? date)
        {
            var query = _context.Schedules
                .Include(x => x.Bus)
                .Include(x => x.Route)
                .Where(x => x.RouteId == routeId);

            if(date.HasValue)
            {
                var utcDate = DateTime.SpecifyKind(date.Value.Date, DateTimeKind.Utc);
                var nextDay = utcDate.AddDays(1);
                query = query.Where(x => x.Departure >= utcDate && x.Departure < nextDay);
            }

            return await query.ToListAsync();
        }
        public async Task<Schedule> CreateAsync(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }
        public async Task<Schedule> UpdateAsync(Schedule schedule)
        {
            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null) { return false; }
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
