// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides basic functionality for configuring an ITokenClient.
    /// </summary>
    public interface ITokenClientBuilder
    {
        /// <summary>
        /// Gets the logical name of this ITokenClient.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the DI services currently configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
