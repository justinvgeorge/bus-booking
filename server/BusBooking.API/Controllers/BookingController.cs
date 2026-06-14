using BusBooking.API.DTOs;
using BusBooking.API.Models;
using BusBooking.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BusBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController: ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _bookingService.GetBookingByIdAsync(id);
                if (result == null) {return NotFound();}
                if (userId != result.UserId) { return StatusCode(403, "Not valid booking"); }
                return Ok(MapToResponseDTO(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);   
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsersBooking()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _bookingService.GetUserBookingsAsync(userId);
                return Ok(result.Select(b => MapToResponseDTO(b)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDTO booking)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _bookingService.CreateBookingAsync(userId, booking.ScheduleId, booking.SeatId);
                return Ok(MapToResponseDTO(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpDelete("{id:int}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _bookingService.CancelBookingAsync(id,userId);
                if (!result) return NotFound("Booking not found or not yours");
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private BookingResponseDTO MapToResponseDTO(Booking booking)
        {
            return new BookingResponseDTO
            {
                Id = booking.Id,
                BookingDate = booking.BookingDate,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status,
                UserFullName = booking.User?.FullName ?? string.Empty,
                DepartureTime = booking.Schedule?.Departure ?? DateTime.MinValue,
                ArrivalTime = booking.Schedule?.Arrival ?? DateTime.MinValue,
                Origin = booking.Schedule?.Route?.Origin ?? string.Empty,
                Destination = booking.Schedule?.Route?.Destination ?? string.Empty,
                BusNumber = booking.Schedule?.Bus?.BusNumber ?? string.Empty,
                SeatNumber = booking.Seat?.SeatNumber ?? string.Empty,
                SeatType = booking.Seat?.SeatType ?? string.Empty
            };
        }
    }
}
