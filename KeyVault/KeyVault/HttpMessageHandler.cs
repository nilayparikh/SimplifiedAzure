#region copyright
/*
Copyright (c) 2017 Nilay Parikh
Modifications Copyright (c) 2017 Nilay Parikh
B: https://blog.nilayparikh.com E: me@nilayparikh.com G: https://github.com/nilayparikh/

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

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
