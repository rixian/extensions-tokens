// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Tokens
{
    using System;

    /// <summary>
    /// An object that represents a token.
    /// </summary>
    internal class TokenInfo : ITokenInfo
    {
        /// <inheritdoc/>
        public string? AccessToken { get; set; }

        /// <inheritdoc/>
        public string? IdentityToken { get; set; }

        /// <inheritdoc/>
        public string? TokenType { get; set; }

        /// <inheritdoc/>
        public string? RefreshToken { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Expiration { get; set; }
    }
}
