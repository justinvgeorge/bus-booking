using BusBooking.API.Models;
using BusBooking.API.Repositories.Interfaces;
using BusBooking.API.Services.Interfaces;

namespace BusBooking.API.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ISeatService _seatService;
        private readonly IScheduleRepository _scheduleRepository;

        public BookingService(IBookingRepository bookingRepository, ISeatRepository seatRepository, ISeatService seatService, IScheduleRepository scheduleRepository)
        {
            _bookingRepository = bookingRepository;
            _seatRepository = seatRepository;
            _seatService = seatService;
            _scheduleRepository = scheduleRepository;
        }
        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
        {
            return await _bookingRepository.GetByUserIdAsync(userId);
        }
        public async Task<Booking> CreateBookingAsync(int userId, int scheduleId, int seatId)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);
            if (seat == null) { throw new Exception("This seat does not exist"); }
            if (!seat.IsAvailable) { throw new Exception("This seat is not available"); }
            var lockOwner = await _seatService.GetSeatLockOwnerAsync(seatId);
            if (lockOwner != userId.ToString()) { throw new Exception("This seat is not available currently"); }
            var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null) { throw new Exception($"{scheduleId} is not available."); };
           
            var booking = new Booking
            {
                UserId = userId,
                ScheduleId = scheduleId,
                SeatId = seatId,
                TotalPrice = schedule.Price

            };
            await _bookingRepository.CreateAsync(booking);

            seat.IsAvailable = false;
            await _seatRepository.UpdateAsync(seat);
            await _seatService.ReleaseLockAsync(seatId);
            return booking;

        }
        public async Task<bool> CancelBookingAsync(int bookingId, int userId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null) { return false; }
            if (booking.UserId != userId) { return false; }

            await _bookingRepository.CancelAsync(bookingId);
            var seat = await _seatRepository.GetByIdAsync(booking.SeatId);
            if(seat != null)
            {
                seat.IsAvailable = true;
                await _seatRepository.UpdateAsync(seat);
            }

            return true;
        }

    }
}
