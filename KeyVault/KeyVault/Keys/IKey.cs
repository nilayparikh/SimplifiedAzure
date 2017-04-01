using System;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault.Models;

namespace EasyAzure.KeyVault.Keys
{
    public interface IKey : IDisposable
    {
        Task<KeyOperationResult> DecryptAsync(string algorithm, byte[] cipherText);
        KeyOperationResult Decrypt(string algorithm, byte[] cipherText);
        KeyOperationResult Encrypt(string algorithm, byte[] plainText);
        Task<KeyOperationResult> EncryptAsync(string algorithm, byte[] plainText);
        KeyBundle GetKey();
        Task<KeyBundle> GetKeyAsync();
        KeyOperationResult Sign(string algorithm, byte[] digest);
        Task<KeyOperationResult> SignAsync(string algorithm, byte[] digest);
        bool Verify(string algorithm, byte[] digest, byte[] signature);
        Task<bool> VerifyAsync(string algorithm, byte[] digest, byte[] signature);
        KeyOperationResult Wrap(byte[] symmetricKey, string algorithm);
        KeyOperationResult Wrap(string symmetricKey, string algorithm);
        Task<KeyOperationResult> UnwrapAsync(byte[] wrappedKey, string algorithm);
        KeyOperationResult Unwrap(byte[] wrappedKey, string algorithm);
        Task<KeyOperationResult> WrapAsync(byte[] symmetricKey, string algorithm);
        Task<KeyOperationResult> WrapAsync(string symmetricKey, string algorithm);
    }
}