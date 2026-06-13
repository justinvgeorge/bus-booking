using BusBooking.API.Data;
using BusBooking.API.Models;
using BusBooking.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.API.Repositories
{
    public class BusRouteRepository : IBusRouteRepository
    {
        private readonly AppDbContext _context;
        public BusRouteRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<IEnumerable<BusRoute>> GetAllAsync()
        {
            return await _context.Routes.ToListAsync();
        }
        public async Task<BusRoute?> GetByIdAsync(int id)
        {
            return await _context.Routes.FindAsync(id);
        }
        public async Task<IEnumerable<BusRoute>> GetByOriginAndDestinationAsync(string origin, string destination)
        {
            return await _context.Routes.Where(x=>x.Origin == origin && x.Destination == destination).ToListAsync();
        }
        public async Task<BusRoute> CreateAsync(BusRoute route)
        {
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();
            return route;
        }
        public async Task<BusRoute> UpdateAsync(BusRoute route)
        {
            _context.Routes.Update(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null) { return false; }
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
