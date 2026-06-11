namespace BusBooking.API.Models
{
    public class Bus
    {
        public int Id { get; set; }
        public string BusNumber { get; set; } = string.Empty;
        public string BusType { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
