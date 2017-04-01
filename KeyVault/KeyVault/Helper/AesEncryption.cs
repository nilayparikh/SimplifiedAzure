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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasyAzure.KeyVault.Helper
{
    public class AesEncryption
    {
        /// <summary>
        /// Generate random bytes
        /// </summary>
        /// <param name="keySize">number of random bytes</param>
        /// <param name="blockSize">blocksize of cipher</param>
        /// <returns></returns>
        public static byte[] GenerateKey(int keySize, int blockSize)
        {
            var aesManaged = new AesManaged {KeySize = keySize, BlockSize = blockSize};
            return aesManaged.Key;
        }

        /// <summary>
        /// Encrypt a byte array using AES 128
        /// </summary>
        /// <param name="key">128 bit key</param>
        /// <param name="secret">byte array that need to be encrypted</param>
        /// <returns>Encrypted array</returns>
        public static byte[] EncryptByteArray(byte[] key, byte[] secret)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (AesManaged cryptor = new AesManaged())
                {
                    cryptor.Mode = CipherMode.CBC;
                    cryptor.Padding = PaddingMode.PKCS7;
                    cryptor.KeySize = 128;
                    cryptor.BlockSize = 128;

                    //We use the random generated iv created by AesManaged
                    byte[] iv = cryptor.IV;

                    using (CryptoStream cs = new CryptoStream(ms, cryptor.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                    {
                        cs.Write(secret, 0, secret.Length);
                    }
                    byte[] encryptedContent = ms.ToArray();

                    //Create new byte array that should contain both unencrypted iv and encrypted data
                    byte[] result = new byte[iv.Length + encryptedContent.Length];

                    //copy our 2 array into one
                    System.Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    System.Buffer.BlockCopy(encryptedContent, 0, result, iv.Length, encryptedContent.Length);

                    return result;
                }
            }
        }

        /// <summary>
        /// Decrypt a byte array using AES 128
        /// </summary>
        /// <param name="key">key in bytes</param>
        /// <param name="secret">the encrypted bytes</param>
        /// <returns>decrypted bytes</returns>
        public static byte[] DecryptByteArray(byte[] key, byte[] secret)
        {
            byte[] iv = new byte[16]; //initial vector is 16 bytes
            byte[] encryptedContent = new byte[secret.Length - 16]; //the rest should be encryptedcontent

            //Copy data to byte array
            System.Buffer.BlockCopy(secret, 0, iv, 0, iv.Length);
            System.Buffer.BlockCopy(secret, iv.Length, encryptedContent, 0, encryptedContent.Length);

            using (MemoryStream ms = new MemoryStream())
            {
                using (AesManaged cryptor = new AesManaged())
                {
                    cryptor.Mode = CipherMode.CBC;
                    cryptor.Padding = PaddingMode.PKCS7;
                    cryptor.KeySize = 128;
                    cryptor.BlockSize = 128;

                    using (CryptoStream cs = new CryptoStream(ms, cryptor.CreateDecryptor(key, iv), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedContent, 0, encryptedContent.Length);

                    }
                    return ms.ToArray();
                }
            }
        }
    }
}
