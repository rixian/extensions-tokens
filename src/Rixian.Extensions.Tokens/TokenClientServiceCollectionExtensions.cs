// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using System.Net.Http;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Rixian.Extensions.Tokens;

    /// <summary>
    /// Provides extensions for registering an ITokenClientFactory with the dependency injection container.
    /// </summary>
    public static class TokenClientServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a new ITokenClient with the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="clientId">The clientId.</param>
        /// <param name="clientSecret">the clientSecret.</param>
        /// <param name="authority">The authority.</param>
        /// <returns>The ITokenClientBuilder.</returns>
        public static ITokenClientBuilder AddTokenClient(this IServiceCollection services, string clientId, string clientSecret, string authority)
        {
            return services.AddTokenClient(
                new TokenClientOptions
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
        public static ITokenClientBuilder AddTokenClient(this IServiceCollection services, TokenClientOptions options)
        {
            services.Configure<InternalTokenClientOptions>(o =>
            {
                o.Authority = options.Authority;
                o.ClientId = options.ClientId;
                o.ClientSecret = options.ClientSecret;
                o.Scope = options.Scope;
                o.RequireHttps = options.RequireHttps;
                o.ValidateIssuerName = options.ValidateIssuerName;
                o.GetHttpClient = svc => svc.GetRequiredService<IHttpClientFactory>().CreateClient();
            });

            services.TryAddSingleton<ITokenClientFactory, TokenClientFactory>();
            return new InternalTokenClientBuilder(Options.Options.DefaultName, services);
        }

        /// <summary>
        /// Registers a new <see cref="ITokenClient"/> with the service collection with a logical name.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="name">The logical name of the <see cref="ITokenClient"/>.</param>
        /// <param name="options">The options for configuring this logical instance of an <see cref="ITokenClient"/>.</param>
        /// <returns>The ITokenClientBuilder.</returns>
        public static ITokenClientBuilder AddTokenClient(this IServiceCollection services, string name, TokenClientOptions options)
        {
            services.Configure<InternalTokenClientOptions>(name, o =>
            {
                o.Authority = options.Authority;
                o.ClientId = options.ClientId;
                o.ClientSecret = options.ClientSecret;
                o.Scope = options.Scope;
                o.RequireHttps = options.RequireHttps;
                o.ValidateIssuerName = options.ValidateIssuerName;
                o.GetHttpClient = svc => svc.GetRequiredService<IHttpClientFactory>().CreateClient();
            });

            services.TryAddSingleton<ITokenClientFactory, TokenClientFactory>();
            return new InternalTokenClientBuilder(name, services);
        }
    }
}
