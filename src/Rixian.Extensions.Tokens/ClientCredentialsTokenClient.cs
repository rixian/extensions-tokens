// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Tokens
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Nito.AsyncEx;

    /// <summary>
    /// Token Client used for the Client Credentials grant type.
    /// </summary>
    internal class ClientCredentialsTokenClient : ITokenClient
    {
        private readonly IServiceProvider services;
        private readonly IHttpClientFactory clientFactory;
        private readonly InternalTokenClientOptions options;
        private readonly object gate = new object();
        private readonly object getTokenGate = new object();

        private ITokenInfo token;
        private DateTimeOffset expiration;

        private AsyncLazy<ITokenInfo> gettingTokenTask = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCredentialsTokenClient"/> class.
        /// </summary>
        /// <param name="services">The DI service provider.</param>
        /// <param name="clientFactory">Used for creating clients when refreshing the token.</param>
        /// <param name="options">Options for the client.</param>
        public ClientCredentialsTokenClient(IServiceProvider services, IHttpClientFactory clientFactory, InternalTokenClientOptions options)
        {
            this.services = services;
            this.clientFactory = clientFactory;
            this.options = options;
        }

        /// <inheritdoc/>
        public Task<ITokenInfo> GetTokenAsync(bool forceRefresh = false)
        {
            lock (this.gate)
            {
                if (forceRefresh)
                {
                    this.token = null;
                }

                if (this.token != null && DateTimeOffset.UtcNow < this.expiration)
                {
                    return Task.FromResult(this.token);
                }

                lock (this.getTokenGate)
                {
                    if (this.gettingTokenTask == null)
                    {
                        this.gettingTokenTask = new AsyncLazy<ITokenInfo>(() => this.GetTokenInternalAsync());
                    }

                    return this.gettingTokenTask.Task;
                }
            }
        }

        private async Task<ITokenInfo> GetTokenInternalAsync()
        {
            try
            {
                HttpClient client = this.options.GetHttpClient?.Invoke(this.services) ?? this.clientFactory.CreateClient();
                IdentityModel.Client.TokenResponse response = await TokenHelpers.GetClientCredentialsTokenAsync(client, this.options).ConfigureAwait(false);

                if (response.IsError)
                {
                    throw response.Exception;
                }

                this.token = TokenHelpers.CreateTokenInfo(response);
                this.expiration = this.token.Expiration.AddMinutes(-5); // Give ourselves a 5 minute buffer
                lock (this.getTokenGate)
                {
                    this.gettingTokenTask = null;
                }

                return this.token;
            }
            catch
            {
                this.token = null;
                this.expiration = DateTimeOffset.MinValue;
                throw;
            }
        }
    }
}
