using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace YeuAI.WebApi.Http.Result
{
    public class ByteArrayResult : IHttpActionResult
    {
        private readonly byte[] _buffer;
        private readonly string _contentType;

        public ByteArrayResult(byte[] buffer, string contentType)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            _buffer = buffer;
            _contentType = contentType;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(_buffer)
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue(_contentType);

            return Task.FromResult(response);
        }
    }
}
