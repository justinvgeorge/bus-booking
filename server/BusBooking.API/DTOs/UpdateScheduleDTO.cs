namespace BusBooking.API.DTOs
{
    public class UpdateScheduleDTO
    {
        public DateTime? DepartureTime {  get; set; }
        public DateTime? ArrivalTime { get; set; }
        public decimal? Price { get; set; }
        public string? Status { get; set; }
    }
}
