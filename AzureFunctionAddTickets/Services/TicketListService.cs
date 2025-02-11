using AzureFunctionAddTickets.Models;
using AzureFunctionAddTickets.ServiceClient;

namespace AzureFunctionAddTickets.Services
{
    public class TicketListService : ITicketListService
    {
        private readonly ITicketListClientService? _ticketListClientService;

        public TicketListService(ITicketListClientService ticketListClientService)
        {
            _ticketListClientService = ticketListClientService;
        }

        public async Task AddTickets(RequestBodyModel[] tickets)
        {
            foreach (var ticketsItem in tickets)
            {

                var response = await (_ticketListClientService ??
                                            throw new ArgumentNullException(nameof(_ticketListClientService))
                                     ).SendTicket(ticketsItem);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error making POST request: {response.StatusCode}");
                }
            }
        }
    }
}
