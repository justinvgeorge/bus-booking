namespace BusBooking.API.DTOs
{
    public class AuthResponseDTO
    {
        public required string Token { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
    }
}
