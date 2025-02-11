using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace AzureFunctionAddTickets.Tests.MSTest.Data
{
    public class FakeHttpResponseData : HttpResponseData
    {
        public FakeHttpResponseData(FunctionContext context) : base(context)
        {
        }

        public override HttpStatusCode StatusCode { get; set; }
        public override Stream Body { get; set; } = new MemoryStream();
#pragma warning disable CS8764 // Nullability of return type doesn't match overridden member (possibly because of nullability attributes).
        public override HttpCookies? Cookies { get; }
#pragma warning restore CS8764 // Nullability of return type doesn't match overridden member (possibly because of nullability attributes).
        public override HttpHeadersCollection Headers { get; set; } = new HttpHeadersCollection();
    }
}
