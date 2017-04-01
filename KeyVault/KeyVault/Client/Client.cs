#region copyright
/*
Copyright (c) 2017 Nilay Parikh
Modifications Copyright (c) 2017 Nilay Parikh
B: https://blog.nilayparikh.com E: me@nilayparikh.com G: https://github.com/nilayparikh/

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace EasyAzure.KeyVault.Client
{
    public class Client : IClient
    {
        public KeyVaultClient KeyVaultClient { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="certificate">X509Certificate Certificate to Authenticate</param>
        public Client(string clientId, X509Certificate2 certificate)
        {
            var assertionCert = new ClientAssertionCertificate(clientId, certificate);
            KeyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
                   (authority, resource, scope) => GetAccessToken(authority, resource, scope, assertionCert)),
                   new HttpMessageHandler());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assertionCert">Certificate used to create client assertion.</param>
        public Client(ClientAssertionCertificate assertionCert)
        {
            KeyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
                   (authority, resource, scope) => GetAccessToken(authority, resource, scope, assertionCert)),
                   new HttpMessageHandler());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keyVaultClient">Instance of Key Vault client.</param>
        public Client(KeyVaultClient keyVaultClient)
        {
            KeyVaultClient = keyVaultClient;
        }

        /// <summary>
        /// Gets the access token
        /// </summary>
        /// <param name="authority"> Authority </param>
        /// <param name="resource"> Resource </param>
        /// <param name="scope"> scope </param>
        /// <param name="assertionCert"></param>
        /// <returns> token </returns>
        private async Task<string> GetAccessToken(string authority, string resource, string scope, ClientAssertionCertificate assertionCert)
        {
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, assertionCert).ConfigureAwait(false);

            return result.AccessToken;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            KeyVaultClient.Dispose();
        }
    }
}
