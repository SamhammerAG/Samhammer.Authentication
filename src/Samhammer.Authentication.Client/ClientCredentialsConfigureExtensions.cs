using System;
using Duende.AccessTokenManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Samhammer.Authentication.Client;

public static class ClientCredentialsConfigureExtensions
{
    public static void AddClientCredentialsOptions<TOptions>(
        this IServiceCollection services,
        string clientName,
        Action<ClientCredentialsClient, TOptions> configureOptions)
        where TOptions : class
    {

        services.AddSingleton<IConfigureOptions<ClientCredentialsClient>>(provider =>
            new ClientCredentialsClientConfigureOptions<TOptions>(clientName, provider, configureOptions));
    }
}
