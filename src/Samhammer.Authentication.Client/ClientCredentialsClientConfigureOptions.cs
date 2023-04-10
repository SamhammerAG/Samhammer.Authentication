using System;
using Duende.AccessTokenManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Samhammer.Authentication.Client;

public class ClientCredentialsClientConfigureOptions<TOptions> : IConfigureNamedOptions<ClientCredentialsClient>
    where TOptions : class
{
    private readonly IServiceProvider serviceProvider;
    private readonly Action<ClientCredentialsClient, TOptions> setupAction;
    private readonly string clientName;

    public ClientCredentialsClientConfigureOptions(
        string clientName,
        IServiceProvider serviceProvider,
        Action<ClientCredentialsClient, TOptions> setupAction)
    {
        this.serviceProvider = serviceProvider;
        this.setupAction = setupAction;
        this.clientName = clientName;
    }

    public void Configure(ClientCredentialsClient options)
    {
        var apiAuthOptionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<TOptions>>();
        apiAuthOptionsMonitor.OnChange(apiAuthOptions =>
        {
            setupAction(options, apiAuthOptions);
        });

        setupAction(options, apiAuthOptionsMonitor.CurrentValue);
    }

    public void Configure(string name, ClientCredentialsClient options)
    {
        if (clientName.Equals(name))
        {
            Configure(options);
        }
    }
}
