// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
namespace Rixian.Extensions.Tokens
{
    using System;
    using System.Collections.Concurrent;
    using System.Net.Http;
    using Microsoft.Extensions.Options;
    using Rixian.Extensions.DependencyInjection;
    using Rixian.Extensions.Errors;
    using static Rixian.Extensions.Errors.Prelude;

    /// <summary>
    /// Factory used for creating instances of an ITokenClient.
    /// </summary>
    internal class ClientCredentialTokenClientFactory : GenericFactory<ClientCredentialsTokenClientOptions, ClientCredentialsTokenClient>, ITokenClientFactory
    {
        private readonly IOptionsMonitor<ClientCredentialsTokenClientOptions> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCredentialTokenClientFactory"/> class.
        /// </summary>
        /// <param name="services">The <see cref="IServiceProvider"/>.</param>
        /// <param name="options">The options to use for creating instances of <see cref="ITokenClient"/>.</param>
        /// <param name="factoryOptions">Options for the factory.</param>
        public ClientCredentialTokenClientFactory(IServiceProvider services, IOptionsMonitor<ClientCredentialsTokenClientOptions> options, IOptions<GenericFactoryOptions<ClientCredentialsTokenClientOptions, ClientCredentialsTokenClient>> factoryOptions)
            : base(services, options, factoryOptions)
        {
            this.options = options;
        }

        /// <inheritdoc/>
        public Result<ITokenClient> GetTokenClient(string name)
        {
            Result<ClientCredentialsTokenClient> result = this.GetItem(name);

            if (result.IsFail)
            {
                return result.Cast<ITokenClient>();
            }
            else
            {
                return Result<ITokenClient>(result.Value);
            }
        }
    }
}
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
