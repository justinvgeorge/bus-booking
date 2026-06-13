using BusBooking.API.Data;
using BusBooking.API.Models;
using BusBooking.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.API.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AppDbContext _context;
        public SeatRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<Seat?> GetByIdAsync(int id)
        {
            return await _context.Seats.Include(x=>x.Bus).FirstOrDefaultAsync(x=>x.Id == id);
        }
        public async Task<IEnumerable<Seat>> GetByBusIdAsync(int busId)
        {
            return await _context.Seats.Where(x => x.BusId == busId).ToListAsync();
        }
        public async Task<IEnumerable<Seat>> GetAvailableSeatsAsync(int busId)
        {
            return await _context.Seats.Where(x => x.BusId == busId && x.IsAvailable).ToListAsync();
        }
        public async Task<Seat> CreateAsync(Seat seat)
        {
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();
            return seat;
        }
        public async Task<Seat> UpdateAsync(Seat seat)
        {
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
            return seat;
        }
    }
}
