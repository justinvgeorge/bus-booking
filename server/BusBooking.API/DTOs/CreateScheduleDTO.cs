namespace BusBooking.API.DTOs
{
    public class CreateScheduleDTO
    {
        public  DateTime DepartureTime { get; set; }
        public  DateTime ArrivalTime { get; set; }
        public Decimal Price { get; set; }
        public int BusId { get; set; }
        public int RouteId { get; set; }
    }
}
