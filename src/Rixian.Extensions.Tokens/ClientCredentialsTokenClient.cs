// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Tokens
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Nito.AsyncEx;
    using Rixian.Extensions.Errors;
    using static Rixian.Extensions.Errors.Prelude;

    /// <summary>
    /// Token Client used for the Client Credentials grant type.
    /// </summary>
    public class ClientCredentialsTokenClient : TokenClientBase
    {
        private readonly IServiceProvider services;
        private readonly IHttpClientFactory clientFactory;
        private readonly ClientCredentialsTokenClientOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCredentialsTokenClient"/> class.
        /// </summary>
        /// <param name="services">The DI service provider.</param>
        /// <param name="clientFactory">Used for creating clients when refreshing the token.</param>
        /// <param name="options">Options for the client.</param>
        public ClientCredentialsTokenClient(IServiceProvider services, IHttpClientFactory clientFactory, ClientCredentialsTokenClientOptions options)
        {
            this.services = services;
            this.clientFactory = clientFactory;
            this.options = options;
        }

        /// <inheritdoc/>
        protected override async Task<Result<ITokenInfo>> GetTokenCoreAsync()
        {
            HttpClient client = this.options.GetBackchannelHttpClient?.Invoke(this.services) ?? this.clientFactory.CreateClient();
            IdentityModel.Client.TokenResponse response = await TokenHelpers.GetClientCredentialsTokenAsync(client, this.options).ConfigureAwait(false);

            if (response.IsError)
            {
                return Error(response.Error, response.ErrorDescription);
            }

            return Result(TokenHelpers.CreateTokenInfo(response));
        }
    }
}
