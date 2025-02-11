namespace AzureFunctionAddTickets.Models
{
    public class RequestBodyModel
    {
        public string? EventName { get; set; }
        public string? Description { get; set; }
        public DateTime? EventDate { get; set; } = DateTime.Today;
    }
}
