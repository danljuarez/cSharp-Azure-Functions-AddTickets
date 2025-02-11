using AzureFunctionAddTickets.Models;
using AzureFunctionAddTickets.ServiceClient;
using Moq;
using Moq.Protected;
using System.Net;

namespace AzureFunctionAddTickets.Tests.MSTest.ServiceClient
{
    [TestClass]
    public class TicketListClientServiceTests
    {
        private readonly Mock<HttpMessageHandler> mockHttpMessageHandler = new();
        private HttpClient? _httpClient;

        [TestInitialize]
        public void Initialize()
        {
            _httpClient = new HttpClient(mockHttpMessageHandler.Object);
        }

        [TestMethod]
        public async Task SendTicket_PostRequestSuccess_Returns_HttpStatusCode_OK()
        {
            // Arrange
            var ticket = new RequestBodyModel()
            {
                EventName = "Event Name Test",
                Description = "Description Test",
                EventDate = DateTime.Now,
            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Success!")
            };

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var ticketListClientService = new TicketListClientService(_httpClient ??
                                                                            throw new ArgumentNullException(nameof(_httpClient))
                                                                     );

            // Act
            var result = await ticketListClientService.SendTicket(ticket);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Success!", await result.Content.ReadAsStringAsync());
        }
    }
}
