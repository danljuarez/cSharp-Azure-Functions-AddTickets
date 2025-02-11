using AzureFunctionAddTickets.Models;
using Newtonsoft.Json;
using System.Text;

namespace AzureFunctionAddTickets.ServiceClient
{
    public class TicketListClientService : ITicketListClientService
    {
        private readonly HttpClient _httpClient;
        private const string ENDPOINT_URL = "http://localhost:5190/api/tickets";

        public TicketListClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(ENDPOINT_URL);
        }

        public async Task<HttpResponseMessage> SendTicket(RequestBodyModel ticket)
        {
            var jsonContent = JsonConvert.SerializeObject(ticket);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync(_httpClient.BaseAddress, content);
        }
    }
}
