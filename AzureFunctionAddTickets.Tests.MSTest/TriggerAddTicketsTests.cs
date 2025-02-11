using AzureFunctionAddTickets.Models;
using AzureFunctionAddTickets.Services;
using AzureFunctionAddTickets.Tests.MSTest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace AzureFunctionAddTickets.Tests.MSTest
{
    [TestClass]
    public class TriggerAddTicketsTests
    {
        private readonly Mock<ILogger<TriggerAddTickets>> loggerMock = new();
        private readonly Mock<ITicketListService> ticketListService = new();

        private MemoryStream? body;
        private Mock<FunctionContext>? context;
        private FakeHttpRequestData? requestData;

        private const string BODY_REQUEST_TEST = "[{\"eventName\":\"Event Name Test\",\"description\": \"Description Test\",\"eventDate\":\"2025-02-02T03:40:49.575Z\"}]";
        private const string URL_TEST = "http://fake.url.com";

        [TestInitialize]
        public void Initialize()
        {
            body = new MemoryStream(Encoding.ASCII.GetBytes(BODY_REQUEST_TEST));
            context = new Mock<FunctionContext>();
            requestData = new FakeHttpRequestData(
                                    context.Object,
                                    new Uri(URL_TEST),
                                    body
                              );
        }

        [TestMethod]
        public async Task Run_PostRequestSuccess_Returns_OkObjectResult()
        {
            // Arrange
            ticketListService
                .Setup(_ => _.AddTickets(It.IsAny<RequestBodyModel[]>()))
                .Verifiable();

            // Act
            var myAzureFunction = new TriggerAddTickets(ticketListService.Object, loggerMock.Object);
            var result = await myAzureFunction.Run(requestData ??
                                                        throw new ArgumentNullException(nameof(requestData))
                                                  );

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Run_PostRequestFailure_Returns_StatusCodeResult_When_HttpRequestException()
        {
            // Arrange
            ticketListService
                .Setup(_ => _.AddTickets(It.IsAny<RequestBodyModel[]>()))
                .ThrowsAsync(new HttpRequestException())
                .Verifiable();

            // Act
            var myAzureFunction = new TriggerAddTickets(ticketListService.Object, loggerMock.Object);
            var result = await myAzureFunction.Run(requestData ??
                                                        throw new ArgumentNullException(nameof(requestData))
                                                  );

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public async Task Run_PostRequestFailure_Returns_BadRequestObjectResult_When_ArgumentNullException()
        {
            // Arrange
            ticketListService
                .Setup(_ => _.AddTickets(It.IsAny<RequestBodyModel[]>()))
                .ThrowsAsync(new ArgumentNullException())
                .Verifiable();
            var expectedResponseMessage = "No request body has been provided!";

            // Act
            var myAzureFunction = new TriggerAddTickets(ticketListService.Object, loggerMock.Object);
            var result = await myAzureFunction.Run(requestData ??
                                                        throw new ArgumentNullException(nameof(requestData))
                                                  );

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual(expectedResponseMessage, ((BadRequestObjectResult)result).Value);
        }

        [TestMethod]
        public async Task Run_PostRequestFailure_Returns_BadRequestObjectResult_When_Exception()
        {
            // Arrange
            ticketListService
                .Setup(_ => _.AddTickets(It.IsAny<RequestBodyModel[]>()))
                .ThrowsAsync(new Exception())
                .Verifiable();
            var expectedResponseMessage = "Unable to process your request!";

            // Act
            var myAzureFunction = new TriggerAddTickets(ticketListService.Object, loggerMock.Object);
            var result = await myAzureFunction.Run(requestData ??
                                                        throw new ArgumentNullException(nameof(requestData))
                                                  );

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual(expectedResponseMessage, ((BadRequestObjectResult)result).Value);
        }

        [TestMethod]
        public async Task Run_PostRequestFailure_Returns_BadRequestObjectResult_When_ticketListService_is_null()
        {
            // Arrange
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var myAzureFunction = new TriggerAddTickets(null, loggerMock.Object); // Note: This null is intended for this test.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // Act
            var result = await myAzureFunction.Run(requestData ??
                                                        throw new ArgumentNullException(nameof(requestData))
                                                  );

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
        }
    }
}
