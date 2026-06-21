using BusBooking.API.DTOs;
using BusBooking.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateAnswer(ChatRequestDTO chatRequestDTO)
        {
            try
            {
                var response = await _chatService.AskAsync(chatRequestDTO.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
