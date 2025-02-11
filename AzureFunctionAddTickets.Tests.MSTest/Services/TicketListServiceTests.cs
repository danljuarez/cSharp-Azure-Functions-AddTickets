using AzureFunctionAddTickets.Models;
using AzureFunctionAddTickets.ServiceClient;
using AzureFunctionAddTickets.Services;
using Moq;
using System.Net;

namespace AzureFunctionAddTickets.Tests.MSTest.Services
{
    [TestClass]
    public class TicketListServiceTests
    {
        private readonly Mock<ITicketListClientService> _ticketListClientService = new();
        private List<RequestBodyModel>? ticketsToAdd;

        [TestInitialize]
        public void Initialize()
        {
            ticketsToAdd = new List<RequestBodyModel>()
            {
                new RequestBodyModel()
                {
                    EventName = "Event Name Test 1",
                    Description = "Description Test 1",
                    EventDate = DateTime.Now,
                },
                new RequestBodyModel()
                {
                    EventName = "Event Name Test 2",
                    Description = "Description Test 2",
                    EventDate = DateTime.Now.AddDays(1),
                }
            };
        }

        [TestMethod]
        public async Task AddTickets_PostRequestSuccess_Returns_HttpStatusCode_OK()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            _ticketListClientService
                .Setup(_ => _.SendTicket(It.IsAny<RequestBodyModel>()))
                .ReturnsAsync(response)
                .Verifiable();

            var ticketListService = new TicketListService(_ticketListClientService.Object);

            // Act
            await ticketListService.AddTickets((ticketsToAdd ??
                                                      throw new ArgumentNullException(nameof(ticketsToAdd))
                                               ).ToArray());

            // Assert
            _ticketListClientService.Verify(_ => _.SendTicket(It.IsAny<RequestBodyModel>()), Times.AtLeast(2));
        }

        [TestMethod]
        public async Task AddTickets_Should_throw_HttpRequestException_When_HttpResponseMessage_is_not_Success()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            _ticketListClientService
                .Setup(_ => _.SendTicket(It.IsAny<RequestBodyModel>()))
                .ReturnsAsync(response);

            var ticketListService = new TicketListService(_ticketListClientService.Object);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => ticketListService.AddTickets((ticketsToAdd ??
                                                                                                            throw new ArgumentNullException(nameof(ticketsToAdd))
                                                                                                       ).ToArray()));
        }

        [TestMethod]
        public async Task AddTickets_Should_throw_ArgumentNullException_When_ticketListClientService_is_null()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            _ticketListClientService
                .Setup(_ => _.SendTicket(It.IsAny<RequestBodyModel>()))
                .ReturnsAsync(response);

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var ticketListService = new TicketListService(null); // Note: This null is intended for this test.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => ticketListService.AddTickets((ticketsToAdd ??
                                                                                                             throw new ArgumentNullException(nameof(ticketsToAdd))
                                                                                                        ).ToArray()));
        }
    }
}
