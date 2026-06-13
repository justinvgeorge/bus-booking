using BusBooking.API.Data;
using BusBooking.API.Models;
using BusBooking.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.API.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings.Include(x=>x.User).Include(x=>x.Schedule).Include(x=>x.Seat).FirstOrDefaultAsync(x=>x.Id == id);
        }
        public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId)
        {
            return await _context.Bookings.Include(x => x.Schedule).Include(x => x.Seat).Where(x=>x.UserId == userId).ToListAsync();
        }
        public async Task<Booking> CreateAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }
        public async Task<Booking> UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return booking;
        }
        public async Task<bool> CancelAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) { return false; }
            booking.Status = "Cancelled";
            _context.Update(booking);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
