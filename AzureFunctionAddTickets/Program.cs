using AzureFunctionAddTickets.ServiceClient;
using AzureFunctionAddTickets.Services;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = FunctionsApplication.CreateBuilder(args);

        builder.ConfigureFunctionsWebApplication();

        // Add DI by registering dependencies directly in the builder (not in startup.cs).
        builder.Services.AddHttpClient();
        builder.Services.AddTransient<ITicketListClientService, TicketListClientService>();
        builder.Services.AddTransient<ITicketListService, TicketListService>();

        builder.Build().Run();
    }
}