// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Tokens
{
    using System.Threading.Tasks;

    /// <summary>
    /// Client interface for retrieving tokens.
    /// </summary>
    public interface ITokenClient
    {
        /// <summary>
        /// Gets a token.
        /// </summary>
        /// <param name="forceRefresh">Forces a clean token if the client uses caching.</param>
        /// <returns>An instance of an <see cref="ITokenInfo"/>.</returns>
        Task<ITokenInfo> GetTokenAsync(bool forceRefresh = false);
    }
}
