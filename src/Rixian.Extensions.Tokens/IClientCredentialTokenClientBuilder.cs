// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using Rixian.Extensions.DependencyInjection;
    using Rixian.Extensions.Tokens;

    /// <summary>
    /// Provides basic functionality for configuring an ITokenClient.
    /// </summary>
    public interface IClientCredentialTokenClientBuilder : IFactoryItemBuilder<ClientCredentialsTokenClientOptions, ClientCredentialsTokenClient>
    {
        /// <summary>
        /// Gets the name of the TokenClientBuilder.
        /// </summary>
        public string Name { get; }
    }
}
