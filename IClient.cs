using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace EasyAzure.KeyVault
{
    public interface IClient : IDisposable
    {
        KeyVaultClient KeyVaultClient { get; }
    }
}