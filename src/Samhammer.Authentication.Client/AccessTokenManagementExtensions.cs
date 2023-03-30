using System;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Samhammer.Authentication.Client
{
    public static class AccessTokenManagementExtensions
    {
        public static ClientAccessTokenManagementOptions AddWithOptionsMonitor<T>(this ClientAccessTokenManagementOptions clientOptions, string clientName, IServiceProvider sp, Func<T, ClientCredentialsTokenRequest> setupClient)
        {
            var apiAuthOptionsMonitor = sp.GetRequiredService<IOptionsMonitor<T>>();
            clientOptions.Clients.Add(clientName, setupClient(apiAuthOptionsMonitor.CurrentValue));

            apiAuthOptionsMonitor.OnChange(apiAuthOptions =>
            {
                clientOptions.Clients.Remove(clientName);
                clientOptions.Clients.Add(clientName, setupClient(apiAuthOptions));
            });

            return clientOptions;
        }
    }
}
