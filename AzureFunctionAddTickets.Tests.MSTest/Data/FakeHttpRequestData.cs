using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Security.Claims;

namespace AzureFunctionAddTickets.Tests.MSTest.Data
{
    public class FakeHttpRequestData : HttpRequestData
    {
        public FakeHttpRequestData(FunctionContext functionContext,
                                   Uri url,
                                   Stream? body = null) : base(functionContext)
        {
            Url = url;
            Body = body ?? new MemoryStream();
            Cookies = new List<IHttpCookie>();
            Identities = new List<ClaimsIdentity>();
            Method = string.Empty;
        }

        public override Uri Url { get; }
        public override Stream Body { get; } = new MemoryStream();
        public override IReadOnlyCollection<IHttpCookie> Cookies { get; }
        public override IEnumerable<ClaimsIdentity> Identities { get; }
        public override string Method { get; }
        public override HttpHeadersCollection Headers { get; } = new HttpHeadersCollection();
        public override HttpResponseData CreateResponse()
        {
            return new FakeHttpResponseData(FunctionContext);
        }
    }
}
