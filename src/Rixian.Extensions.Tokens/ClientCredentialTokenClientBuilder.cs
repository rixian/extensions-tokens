// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Internal implementation of the ITokenClientBuilder interface.
    /// </summary>
    internal class ClientCredentialTokenClientBuilder : IClientCredentialTokenClientBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCredentialTokenClientBuilder"/> class.
        /// </summary>
        /// <param name="services">The DI services currently configured.</param>
        /// <param name="name">The name of the client credential builder.</param>
        public ClientCredentialTokenClientBuilder(IServiceCollection services, string name)
        {
            this.Services = services;
            this.Name = name;
        }

        /// <inheritdoc/>
        public IServiceCollection Services { get; }

        /// <inheritdoc/>
        public string Name { get; }
    }
}
