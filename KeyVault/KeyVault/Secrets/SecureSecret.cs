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

using System;
using System.Threading.Tasks;
using SimplifiedAzure.KeyVault.Client;
using SimplifiedAzure.KeyVault.Helper;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;

namespace SimplifiedAzure.KeyVault.Secrets
{
    public class SecureSecret : Secret, ISecureSecret
    {
        private readonly IClient _client;
        private readonly string _secretId;
        private readonly string _secureKeyId;
        private readonly string _secureSecretId;

        public SecureSecret(IClient client, string secretId, string secureSecretId, string secureKeyId)
            : base(client, secretId)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (secretId == null) throw new ArgumentNullException(nameof(secretId));
            if (secureSecretId == null) throw new ArgumentNullException(nameof(secureSecretId));
            if (secureKeyId == null) throw new ArgumentNullException(nameof(secureKeyId));

            _client = client;
            _secretId = secretId;
            _secureSecretId = secureSecretId;
            _secureKeyId = secureKeyId;
        }

        #region GetSecureSecret

        public async Task<SecretBundle> GetSecureSecretAsync()
        {
            var encryptedSecureSecret = await _client.KeyVaultClient.GetSecretAsync(_secureSecretId);
            var secureSecret = await _client.KeyVaultClient.UnwrapKeyAsync(_secureKeyId, encryptedSecureSecret.Tags["a"],
                Convert.FromBase64String(encryptedSecureSecret.Value));

            var encryptedSecert = await _client.KeyVaultClient.GetSecretAsync(_secretId);
            var secret = AesEncryption.DecryptByteArray(secureSecret.Result,
                Convert.FromBase64String(encryptedSecert.Value));

            encryptedSecert.Value = Convert.ToBase64String(secret);
            return encryptedSecert;
        }

        public SecretBundle GetSecureSecret()
        {
            return GetSecureSecretAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion
    }
}