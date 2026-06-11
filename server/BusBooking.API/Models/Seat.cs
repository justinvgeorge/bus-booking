namespace BusBooking.API.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public string SeatType { get; set; } = string.Empty ;
        public bool IsAvailable { get; set; } = true;
        public int BusId { get; set; }
        public Bus Bus { get; set; } = null!;
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
