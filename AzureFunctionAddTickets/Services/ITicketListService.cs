using AzureFunctionAddTickets.Models;

namespace AzureFunctionAddTickets.Services
{
    public interface ITicketListService
    {
        Task AddTickets(RequestBodyModel[] tickets);
    }
}