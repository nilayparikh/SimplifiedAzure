using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyAzure.KeyVault
{
    public class HttpMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// Adds the Host header to every request if the "KmsNetworkUrl" configuration setting is specified.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var requestUri = request.RequestUri;
            var targetUri = requestUri;
            var authority = targetUri.Authority;

            request.Headers.Add("Host", authority);

            return
                base.SendAsync(request, cancellationToken)
                    .ContinueWith<HttpResponseMessage>(response => response.Result, cancellationToken);
        }
    }
}
