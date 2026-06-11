namespace BusBooking.API.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime Departure {  get; set; }
        public DateTime Arrival { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = "Scheduled";
        public int BusId { get; set; }
        public int RouteId { get; set; }
        public Bus Bus { get; set; } = null!;
        public Route Route { get; set; } = null!;
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
