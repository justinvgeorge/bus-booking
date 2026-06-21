using BusBooking.API.DTOs;

namespace BusBooking.API.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatResponseDTO> AskAsync(string message);
    }
}
