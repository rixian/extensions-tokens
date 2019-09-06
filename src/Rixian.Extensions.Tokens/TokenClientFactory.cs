// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
namespace Rixian.Extensions.Tokens
{
    using System;
    using System.Collections.Concurrent;
    using System.Net.Http;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Factory used for creating instances of an ITokenClient.
    /// </summary>
    internal class TokenClientFactory : ITokenClientFactory
    {
        private readonly IServiceProvider services;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IOptionsMonitor<InternalTokenClientOptions> options;
        private readonly ConcurrentDictionary<string, ITokenClient> tokenClients = new ConcurrentDictionary<string, ITokenClient>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenClientFactory"/> class.
        /// </summary>
        /// <param name="services">The <see cref="IServiceProvider"/>.</param>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/>.</param>
        /// <param name="options">The options to use for creating instances of <see cref="ITokenClient"/>.</param>
        public TokenClientFactory(IServiceProvider services, IHttpClientFactory httpClientFactory, IOptionsMonitor<InternalTokenClientOptions> options)
        {
            this.services = services;
            this.httpClientFactory = httpClientFactory;
            this.options = options;
        }

        /// <inheritdoc/>
        public ITokenClient GetTokenClient(string name)
        {
            return this.tokenClients.GetOrAdd(name, n =>
            {
                return new ClientCredentialsTokenClient(this.services, this.httpClientFactory, this.options.Get(n));
            });
        }
    }
}
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
