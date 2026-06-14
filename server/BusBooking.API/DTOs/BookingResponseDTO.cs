namespace BusBooking.API.DTOs
{
    public class BookingResponseDTO
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }= string.Empty;
        public string UserFullName { get; set; }=string.Empty;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string Origin {  get; set; }=string.Empty;
        public string Destination {  get; set; }=string.Empty;
        public string BusNumber {  get; set; }=string.Empty;
        public string SeatNumber {  get; set; }=string.Empty;
        public string SeatType {  get; set; }=string.Empty;
    }
}
