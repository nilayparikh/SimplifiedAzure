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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyAzure.KeyVault.Client;
using EasyAzure.KeyVault.Keys;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.KeyVault.WebKey;

namespace EasyAzure.KeyVault.Secrets
{
    public class Secret : ISecret
    {
        private readonly IClient _client;
        private readonly string _secretId;

        /// <summary>
        /// Create an instance of secret
        /// </summary>
        /// <param name="client">EasyAzure.KeyVault IClient instance</param>
        /// <param name="secretId">SecretId</param>
        public Secret(IClient client, string secretId)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (secretId == null) throw new ArgumentNullException(nameof(secretId));

            _client = client;
            _secretId = secretId;
        }

        #region GetSecret

        public async Task<SecretBundle> GetSecretAsync()
        {
            return await _client.KeyVaultClient.GetSecretAsync(_secretId);
        }

        public SecretBundle GetSecret()
        {
            return GetSecretAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion
    }
}
