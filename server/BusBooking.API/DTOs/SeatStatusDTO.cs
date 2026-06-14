namespace BusBooking.API.DTOs
{
    public class SeatStatusDTO
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public string SeatType { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public bool IsLocked { get; set; }
        public string? LockedByUserId { get; set; }
    }
}
