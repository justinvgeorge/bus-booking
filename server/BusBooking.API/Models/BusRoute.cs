namespace BusBooking.API.Models
{
    public class BusRoute
    {
        public int Id { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; }= string.Empty;
        public decimal Distance {get; set; }
        public TimeSpan Duration { get; set; }
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
