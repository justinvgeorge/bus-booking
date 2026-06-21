namespace BusBooking.API.DTOs
{
        public class ChatRequestDTO
        {
            public required string Message { get; set; }
        }

        public class ChatResponseDTO 
        { 
            public string Response { get; set; } = string.Empty;
        }

}
