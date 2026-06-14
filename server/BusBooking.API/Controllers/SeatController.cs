using BusBooking.API.DTOs;
using BusBooking.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BusBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;
        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }
        [HttpGet("{busId:int}/seats")]
        public async Task<IActionResult> GetAllSeatsByBusId(int busId)
        {
            try
            {
                var seats = await _seatService.GetSeatsByBusIdAsync(busId);
                var seatStatusList = new List<SeatStatusDTO>();
                foreach (var seat in seats) 
                {
                    var isLocked = await _seatService.IsSeatLockedAsync(seat.Id);
                    var lockOwner = await _seatService.GetSeatLockOwnerAsync(seat.Id);

                    seatStatusList.Add(new SeatStatusDTO
                    {
                        Id = seat.Id,
                        SeatNumber = seat.SeatNumber,
                        SeatType = seat.SeatType,
                        IsAvailable = seat.IsAvailable,
                        IsLocked = isLocked,
                        LockedByUserId  =  lockOwner
                    });
                }
                return Ok(seatStatusList);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPost("{seatId:int}/lock")]
        public async Task<IActionResult> LockSeat(int seatId)
        {
            try
            { 
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _seatService.LockSeatAsync(seatId, userId);
                if (!result)
                {
                    return Conflict("This seat is already locked by another user");
                }
                return Ok("Seat locked successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{seatId:int}/lock")]
        public async Task<IActionResult> DeleteSeatLock(int seatId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var lockOwner = await _seatService.GetSeatLockOwnerAsync(seatId);
                if(lockOwner == null) 
                    return NotFound("No lock exists for this seat");
                if (lockOwner != userId.ToString())
                    return Forbid();

                await _seatService.ReleaseLockAsync(seatId);
                return Ok("Seat lock released successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
