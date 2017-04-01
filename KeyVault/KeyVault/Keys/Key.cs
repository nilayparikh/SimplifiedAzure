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
using System.Text;
using System.Threading.Tasks;
using EasyAzure.KeyVault.Client;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;

namespace EasyAzure.KeyVault.Keys
{
    public class Key : IKey
    {
        private readonly IClient _client;
        private readonly string _keyId;

        /// <summary>
        /// Create an instance of key
        /// </summary>
        /// <param name="client">EasyAzure.KeyVault Client</param>
        /// <param name="keyId">KeyId</param>
        public Key(IClient client, string keyId)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (keyId == null) throw new ArgumentNullException(nameof(keyId));

            _client = client;
            _keyId = keyId;
        }

        #region GetKey

        /// <summary>
        /// Gets the specified key
        /// </summary>
        /// <returns> retrieved key bundle </returns>
        public async Task<KeyBundle> GetKeyAsync()
        {
            return await _client.KeyVaultClient.GetKeyAsync(_keyId);
        }

        /// <summary>
        /// Gets the specified key
        /// </summary>
        /// <returns>Retrieved key bundle </returns>
        public KeyBundle GetKey()
        {
            return GetKeyAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion

        #region WrapKey

        public async Task<KeyOperationResult> WrapAsync(string symmetricKey, string algorithm)
        {
            return await WrapAsync(Encoding.UTF8.GetBytes(symmetricKey), algorithm);
        }

        public async Task<KeyOperationResult> WrapAsync(byte[] symmetricKey, string algorithm)
        {
            if (symmetricKey == null) throw new ArgumentNullException(nameof(symmetricKey));
            if (algorithm == null) throw new ArgumentNullException(nameof(algorithm));

            return await _client.KeyVaultClient.WrapKeyAsync(_keyId, algorithm, symmetricKey);
        }

        public KeyOperationResult Wrap(string symmetricKey, string algorithm)
        {
            return
                WrapAsync(Encoding.UTF8.GetBytes(symmetricKey), algorithm)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
        }

        public KeyOperationResult Wrap(byte[] symmetricKey, string algorithm)
        {
            return WrapAsync(symmetricKey, algorithm).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion

        #region UnwarpKey

        public async Task<KeyOperationResult> UnwrapAsync(byte[] wrappedKey, string algorithm)
        {
            if (wrappedKey == null) throw new ArgumentNullException(nameof(wrappedKey));
            if (algorithm == null) throw new ArgumentNullException(nameof(algorithm));

            return await _client.KeyVaultClient.UnwrapKeyAsync(_keyId, algorithm, wrappedKey);
        }

        public KeyOperationResult Unwrap(byte[] wrappedKey, string algorithm)
        {
            return UnwrapAsync(wrappedKey, algorithm).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion

        #region Encrypt

        public async Task<KeyOperationResult> EncryptAsync(string algorithm, byte[] plainText)
        {
            if (algorithm == null) throw new ArgumentNullException(nameof(algorithm));
            if (plainText == null) throw new ArgumentNullException(nameof(plainText));

            return await _client.KeyVaultClient.EncryptAsync(_keyId, algorithm, plainText);
        }

        public KeyOperationResult Encrypt(string algorithm, byte[] plainText)
        {
            return EncryptAsync(algorithm, plainText).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion

        #region Decrypt

        public async Task<KeyOperationResult> DecryptAsync(string algorithm, byte[] cipherText)
        {
            if (algorithm == null) throw new ArgumentNullException(nameof(algorithm));
            if (cipherText == null) throw new ArgumentNullException(nameof(cipherText));

            return await _client.KeyVaultClient.DecryptAsync(_keyId, algorithm, cipherText);
        }

        public KeyOperationResult Decrypt(string algorithm, byte[] cipherText)
        {
            return DecryptAsync(algorithm, cipherText).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion

        #region Sign

        public async Task<KeyOperationResult> SignAsync(string algorithm, byte[] digest)
        {
            if (algorithm == null) throw new ArgumentNullException(nameof(algorithm));
            if (digest == null) throw new ArgumentNullException(nameof(digest));

            return await _client.KeyVaultClient.SignAsync(_keyId, algorithm, digest);
        }

        public KeyOperationResult Sign(string algorithm, byte[] digest)
        {
            return SignAsync(algorithm, digest).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion

        #region Verify

        public async Task<bool> VerifyAsync(string algorithm, byte[] digest, byte[] signature)
        {
            if (algorithm == null) throw new ArgumentNullException(nameof(algorithm));
            if (digest == null) throw new ArgumentNullException(nameof(digest));
            if (signature == null) throw new ArgumentNullException(nameof(signature));

            return await _client.KeyVaultClient.VerifyAsync(_keyId, algorithm, digest, signature);
        }

        public bool Verify(string algorithm, byte[] digest, byte[] signature)
        {
            return VerifyAsync(algorithm, digest, signature).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
