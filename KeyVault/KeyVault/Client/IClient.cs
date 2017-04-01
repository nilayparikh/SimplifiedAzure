using System;
using Microsoft.Azure.KeyVault;

namespace EasyAzure.KeyVault.Client
{
    public interface IClient : IDisposable
    {
        KeyVaultClient KeyVaultClient { get; }
    }
}