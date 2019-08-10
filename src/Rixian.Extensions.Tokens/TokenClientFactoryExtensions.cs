// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Tokens
{
    using System;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Provides extensions for the ITokenClientFactory interface.
    /// </summary>
    public static class TokenClientFactoryExtensions
    {
        /// <summary>
        /// Gets an instance of an ITokenClient using the default logical name provided by Microsoft.Extensions.Options.Optons.DefaultName.
        /// </summary>
        /// <param name="factory">The factory to use for creating insances of an ITokenClient.</param>
        /// <returns>An instance of an ITokenClient.</returns>
        public static ITokenClient GetTokenClient(this ITokenClientFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return factory.GetTokenClient(Options.DefaultName);
        }
    }
}
