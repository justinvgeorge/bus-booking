namespace BusBooking.API.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Confirmed";
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; } = null!;
        public int SeatId { get; set; }
        public Seat Seat { get; set; } = null!;
    }
}
