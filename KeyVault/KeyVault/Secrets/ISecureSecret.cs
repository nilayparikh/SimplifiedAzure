using System.Threading.Tasks;
using Microsoft.Azure.KeyVault.Models;

namespace EasyAzure.KeyVault.Secrets
{
    public interface ISecureSecret : ISecret
    {
        SecretBundle GetSecureSecret();
        Task<SecretBundle> GetSecureSecretAsync();
    }
}