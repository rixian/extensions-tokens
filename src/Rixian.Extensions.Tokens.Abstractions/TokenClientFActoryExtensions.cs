// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Tokens
{
    using System.Threading.Tasks;
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Extension methods for working with the ITokenClient interface.
    /// </summary>
    public static class TokenClientFactoryExtensions
    {
        /// <summary>
        /// Creates an instance of an ITokenClient using the given logical name.
        /// </summary>
        /// <param name="tokenClientFactory">The ITokenClientFactory instance.</param>
        /// <returns>An instance of an ITokenClient.</returns>
        public static Result<ITokenClient> GetTokenClient(this ITokenClientFactory tokenClientFactory)
        {
            if (tokenClientFactory is null)
            {
                throw new System.ArgumentNullException(nameof(tokenClientFactory));
            }

            return tokenClientFactory.GetTokenClient(string.Empty);
        }
    }
}
