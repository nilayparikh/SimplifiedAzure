using System.Threading.Tasks;
using Microsoft.Azure.KeyVault.Models;

namespace EasyAzure.KeyVault.Secrets
{
    public interface ISecret
    {
        SecretBundle GetSecret();
        Task<SecretBundle> GetSecretAsync();
    }
}