using BusBooking.API.DTOs;
using BusBooking.API.Repositories.Interfaces;
using BusBooking.API.Services.Interfaces;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;

namespace BusBooking.API.Services
{
    public class ChatService : IChatService
    {

        private readonly IScheduleRepository _scheduleRepository;
        private readonly IBusRouteRepository _busRouteRepository;
        private readonly HttpClient _httpClient;
        public ChatService(IScheduleRepository scheduleRepository, IBusRouteRepository busRouteRepository, IHttpClientFactory httpClientFactory) 
        {
            _scheduleRepository = scheduleRepository;
            _busRouteRepository = busRouteRepository;
            _httpClient = httpClientFactory.CreateClient("Ollama");

        }

        public async Task<ChatResponseDTO> AskAsync(string message)
        {
            string context = await BuildContextAsync();
            var prompt = BuildPrompt(message, context);
            var response = await CallOllamaAsync(prompt);

            return new ChatResponseDTO { Response = response};
        }

        private async Task<string> BuildContextAsync()
        {
            var busRoutes = await _busRouteRepository.GetAllAsync();
            var schedules = await _scheduleRepository.GetAllAsync();

            var sb = new StringBuilder();

            sb.AppendLine("Available Routes:");
            foreach (var busRoute in busRoutes)
            {
                sb.AppendLine($"- {busRoute.Origin} to {busRoute.Destination} ({busRoute.Distance}km)");
            }
            sb.AppendLine("\nAvailable Schedules:");
            foreach (var schedule in schedules.Take(20))
            {
                sb.AppendLine($"- {schedule.Route?.Origin} to {schedule.Route?.Destination} | Bus: {schedule.Bus?.BusNumber} ({schedule.Bus?.BusType}) | Departure: {schedule.Departure:yyyy-MM-dd HH:mm} | Price: ₹{schedule.Price} | Status: {schedule.Status} ");
            }

            return sb.ToString();

        }

        private string BuildPrompt(string message, string context)
        {
            var sb = new StringBuilder();
            sb.AppendLine("You are a helpful bus booking assistant for BusBook application.");
            sb.AppendLine("Use the following data to answer the user's question accurately.");
            sb.AppendLine("Only use the proivded data - do not make up information");
            sb.AppendLine("Keep your answer consise and friendly");
            sb.AppendLine("");
            sb.AppendLine("--- BUS BOOOKING DATA ---");
            sb.AppendLine(context);
            sb.AppendLine("--- END OF DATA ---");
            sb.AppendLine($"User question: {message}");
            sb.AppendLine("Answer:");
            return sb.ToString();
        }

        private async Task<string> CallOllamaAsync(string prompt)
        {
            var request = new
            {
                model = "llama3.2",
                prompt = prompt,
                stream = false
            };

            var response = await _httpClient.PostAsJsonAsync("/api/generate", request);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<JsonElement>(json);
            return result.GetProperty("response").GetString() ?? "Sorry, I could not generate the response.";

        }
    }
}
