// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Internal implementation of the ITokenClientBuilder interface.
    /// </summary>
    internal class InternalTokenClientBuilder : ITokenClientBuilder
    {
        private readonly IServiceCollection services;
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalTokenClientBuilder"/> class.
        /// </summary>
        /// <param name="name">The logical name of this ITokenClient.</param>
        /// <param name="services">The DI services currently configured.</param>
        public InternalTokenClientBuilder(string name, IServiceCollection services)
        {
            this.name = name;
            this.services = services;
        }

        /// <inheritdoc/>
        public string Name => this.name;

        /// <inheritdoc/>
        public IServiceCollection Services => this.services;
    }
}
