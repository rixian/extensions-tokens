// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Tokens
{
    using System.Threading.Tasks;
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Extension methods for working with the ITokenClient interface.
    /// </summary>
    public static class TokenClientExtensions
    {
        /// <summary>
        /// Gets a token.
        /// </summary>
        /// <param name="tokenClient">The ITokenClient.</param>
        /// <returns>An instance of an <see cref="ITokenInfo"/>.</returns>
        public static Task<Result<ITokenInfo>> GetTokenAsync(this ITokenClient tokenClient)
        {
            if (tokenClient is null)
            {
                throw new System.ArgumentNullException(nameof(tokenClient));
            }

            return tokenClient.GetTokenAsync(false);
        }
    }
}
