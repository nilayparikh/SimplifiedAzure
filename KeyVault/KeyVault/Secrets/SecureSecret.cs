using System;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using EasyAzure.KeyVault.Client;
using EasyAzure.KeyVault.Keys;
using System.Threading.Tasks;
using EasyAzure.KeyVault.Helper;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.KeyVault.WebKey;

namespace EasyAzure.KeyVault.Secrets
{
    public class SecureSecret : Secret, ISecureSecret
    {
        private readonly IClient _client;
        private readonly string _secretId;
        private readonly string _secureSecretId;
        private readonly string _secureKeyId;

        private readonly Lazy<IKey> _key;

        public SecureSecret(IClient client, string secretId, string secureSecretId, string secureKeyId) : base(client, secretId)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (secretId == null) throw new ArgumentNullException(nameof(secretId));
            if (secureSecretId == null) throw new ArgumentNullException(nameof(secureSecretId));
            if (secureKeyId == null) throw new ArgumentNullException(nameof(secureKeyId));

            _client = client;
            _secretId = secretId;
            _secureSecretId = secureSecretId;
            _secureKeyId = secureKeyId;

            _key = new Lazy<IKey>(() => new Key(_client, _secureKeyId), LazyThreadSafetyMode.ExecutionAndPublication);
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
