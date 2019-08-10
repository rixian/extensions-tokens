// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Tokens
{
    using System;

    /// <summary>
    /// An object that represents a token.
    /// </summary>
    public interface ITokenInfo
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// Gets the identity token.
        /// </summary>
        string IdentityToken { get; }

        /// <summary>
        /// Gets the token type.
        /// </summary>
        string TokenType { get; }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        string RefreshToken { get; }

        /// <summary>
        /// Gets the token expiration as a point in time.
        /// </summary>
        DateTimeOffset Expiration { get; }
    }
}
