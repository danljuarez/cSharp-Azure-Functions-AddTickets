
using AzureFunctionAddTickets.Models;

namespace AzureFunctionAddTickets.ServiceClient
{
    public interface ITicketListClientService
    {
        Task<HttpResponseMessage> SendTicket(RequestBodyModel ticket);
    }
}