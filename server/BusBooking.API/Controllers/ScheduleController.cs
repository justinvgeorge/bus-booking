using BusBooking.API.DTOs;
using BusBooking.API.Models;
using BusBooking.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace BusBooking.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSchedule()
        {
            try
            {
                var result = await _scheduleService.GetAllScheduleAsync();
                return Ok(result.Select(s=> MapToResponseDTO(s)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            try
            {
                var result = await _scheduleService.GetScheduleByIdAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(MapToResponseDTO(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> GetSearchSchedule([FromQuery] string origin, [FromQuery] string destination, [FromQuery] DateTime date)
        {
            try
            {
                var result = await _scheduleService.SearchSchedulesAsync(origin, destination, date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CraetaSchedule(CreateScheduleDTO dto)
        {
            try
            {
                var schedule = new Schedule
                {
                    Departure = DateTime.SpecifyKind(dto.DepartureTime, DateTimeKind.Utc),
                    Arrival = DateTime.SpecifyKind(dto.ArrivalTime, DateTimeKind.Utc),
                    Price = dto.Price,
                    BusId = dto.BusId,
                    RouteId = dto.RouteId
                };
                await _scheduleService.CreateScheduleAsync(schedule);
                var result = await _scheduleService.GetScheduleByIdAsync(schedule.Id);
                return Ok(MapToResponseDTO(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSchedule(UpdateScheduleDTO dto, int id)
        {
            try
            {
                var existingSchedule = await _scheduleService.GetScheduleByIdAsync(id);
                if (existingSchedule == null)
                {
                    return BadRequest("Schedule ID does not exist");

                }
                if (dto.DepartureTime.HasValue) existingSchedule.Departure = DateTime.SpecifyKind(dto.DepartureTime.Value, DateTimeKind.Utc);
                if (dto.ArrivalTime.HasValue) existingSchedule.Arrival = DateTime.SpecifyKind(dto.ArrivalTime.Value, DateTimeKind.Utc);
                if (dto.Price.HasValue) existingSchedule.Price = dto.Price.Value;
                if (dto.Status != null) existingSchedule.Status = dto.Status;

                await _scheduleService.UpdateScheduleAsync(existingSchedule);
                return Ok(MapToResponseDTO(existingSchedule));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            try
            {
                var existingSchedule = await _scheduleService.GetScheduleByIdAsync(id);
                if (existingSchedule == null)
                {
                    return BadRequest("Schedule ID does not exist");

                }

                var result = await _scheduleService.DeleteScheduleAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private ScheduleResponseDTO MapToResponseDTO(Schedule schedule)
        {
            return new ScheduleResponseDTO
            {
                Id = schedule.Id,
                Departure = schedule.Departure,
                Arrival = schedule.Arrival,
                Price = schedule.Price,
                Status = schedule.Status,
                BusNumber = schedule.Bus?.BusNumber ?? string.Empty,
                BusType = schedule.Bus?.BusType ?? string.Empty,
                Origin = schedule.Route?.Origin ?? string.Empty,
                Destination = schedule.Route?.Destination ?? string.Empty
            };
        }



    }
}
