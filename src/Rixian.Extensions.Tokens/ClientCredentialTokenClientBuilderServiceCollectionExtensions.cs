// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using System.Net.Http;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Rixian.Extensions.DependencyInjection;
    using Rixian.Extensions.Tokens;

    /// <summary>
    /// Provides extensions for registering an ITokenClientFactory with the dependency injection container.
    /// </summary>
    public static class ClientCredentialTokenClientBuilderServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a new ITokenClient with the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="clientId">The clientId.</param>
        /// <param name="clientSecret">the clientSecret.</param>
        /// <param name="authority">The authority.</param>
        /// <returns>The ITokenClientBuilder.</returns>
        public static IClientCredentialTokenClientBuilder AddClientCredentialsTokenClient(this IServiceCollection services, string clientId, string clientSecret, string authority)
        {
            return services.AddClientCredentialsTokenClient(
                new ClientCredentialsTokenClientOptions
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    Authority = authority,
                    Scope = null,
                    RequireHttps = true,
                    ValidateIssuerName = true,
                });
        }

        /// <summary>
        /// Registers a new <see cref="ITokenClient"/> with the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="options">The options for configuring this logical instance of an <see cref="ITokenClient"/>.</param>
        /// <returns>The ITokenClientBuilder.</returns>
        public static IClientCredentialTokenClientBuilder AddClientCredentialsTokenClient(this IServiceCollection services, ClientCredentialsTokenClientOptions options)
        {
            return services.AddClientCredentialsTokenClientFactory(Options.Options.DefaultName)
                .Configure<ClientCredentialsTokenClientOptions, ClientCredentialsTokenClient, IClientCredentialTokenClientBuilder>(o =>
                {
                    o.Authority = options.Authority;
                    o.ClientId = options.ClientId;
                    o.ClientSecret = options.ClientSecret;
                    o.Scope = options.Scope;
                    o.RequireHttps = options.RequireHttps;
                    o.ValidateIssuerName = options.ValidateIssuerName;
                    o.GetBackchannelHttpClient = options.GetBackchannelHttpClient ?? (svc => svc.GetRequiredService<IHttpClientFactory>().CreateClient());
                });
        }

        /// <summary>
        /// Registers a new <see cref="ITokenClient"/> with the service collection with a logical name.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="name">The logical name of the <see cref="ITokenClient"/>.</param>
        /// <param name="options">The options for configuring this logical instance of an <see cref="ITokenClient"/>.</param>
        /// <returns>The ITokenClientBuilder.</returns>
        public static IClientCredentialTokenClientBuilder AddClientCredentialsTokenClient(this IServiceCollection services, string name, ClientCredentialsTokenClientOptions options)
        {
            return services.AddClientCredentialsTokenClientFactory(name)
                .Configure<ClientCredentialsTokenClientOptions, ClientCredentialsTokenClient, IClientCredentialTokenClientBuilder>(name, o =>
                {
                    o.Authority = options.Authority;
                    o.ClientId = options.ClientId;
                    o.ClientSecret = options.ClientSecret;
                    o.Scope = options.Scope;
                    o.RequireHttps = options.RequireHttps;
                    o.ValidateIssuerName = options.ValidateIssuerName;
                    o.GetBackchannelHttpClient = options.GetBackchannelHttpClient ?? (svc => svc.GetRequiredService<IHttpClientFactory>().CreateClient());
                });
        }

        /// <summary>
        /// Registers a new <see cref="ITokenClient"/> with the service collection with a logical name.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="name">The logical name of the <see cref="ITokenClient"/>.</param>
        /// <returns>The ITokenClientBuilder.</returns>
        public static IClientCredentialTokenClientBuilder AddClientCredentialsTokenClientFactory(this IServiceCollection services, string name)
        {
            services.TryAddSingleton<ClientCredentialTokenClientFactory>();
            services.TryAddSingleton<ITokenClientFactory>(svc => svc.GetRequiredService<ClientCredentialTokenClientFactory>());
            services.TryAddSingleton<IFactory<ClientCredentialsTokenClientOptions, ClientCredentialsTokenClient>>(svc => svc.GetRequiredService<ClientCredentialTokenClientFactory>());
            services.ConfigureFactory<ClientCredentialsTokenClientOptions, ClientCredentialsTokenClient>((svc, o) => new ClientCredentialsTokenClient(svc, svc.GetRequiredService<IHttpClientFactory>(), o!));
            return new ClientCredentialTokenClientBuilder(services, name);
        }
    }
}
