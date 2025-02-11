using AzureFunctionAddTickets.Models;
using AzureFunctionAddTickets.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net;

namespace AzureFunctionAddTickets
{
    public class TriggerAddTickets
    {
        private static ILogger<TriggerAddTickets>? _logger;
        private readonly ITicketListService? _ticketListService;

        public TriggerAddTickets(ITicketListService ticketListService,
                                 ILogger<TriggerAddTickets> logger)
        {
            _ticketListService = ticketListService;
            _logger = logger;
        }

        [Function("TriggerAddTickets")]

        [OpenApiOperation(operationId: "Run")]

        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]

        [OpenApiRequestBody("application/json", typeof(RequestBodyModel[]),
            Description = "JSON request body containing following structure:")]

        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string),
            Description = "The OK response message containing a JSON result.")]

        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req)
        {
            _logger?.LogInformation("C# HTTP trigger function is processing a request.");

            try
            {
                string? requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                RequestBodyModel[] data = JsonConvert.DeserializeObject<RequestBodyModel[]>(requestBody) ?? throw new ArgumentNullException();

                await (_ticketListService ??
                            throw new ArgumentNullException(nameof(_ticketListService))
                      ).AddTickets(data);

                _logger?.LogInformation(requestBody);

                return new OkObjectResult("Your C# HTTP trigger function has been processed!");
            }
            catch (ArgumentNullException ex)
            {
                var errorMessage = "No request body has been provided!";
                _logger?.LogError($"C# HTTP trigger function - {errorMessage} Description: {ex.Message}");

                return new BadRequestObjectResult(errorMessage);
            }
            catch (HttpRequestException ex)
            {
                _logger?.LogError($"Error making POST request: {ex.Message}");

                return new StatusCodeResult(500);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"C# HTTP trigger function error - Description: {ex.Message}");

                return new BadRequestObjectResult("Unable to process your request!");
            }
        }
    }
}
