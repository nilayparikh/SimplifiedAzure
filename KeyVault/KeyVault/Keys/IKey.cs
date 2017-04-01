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