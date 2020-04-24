// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Tokens
{
    using System;
    using System.Net.Http;

    /// <summary>
    /// Options for the token client.
    /// </summary>
    public class ClientCredentialsTokenClientOptions
    {
        /// <summary>
        /// Gets or sets the authority to use.
        /// </summary>
        public string? Authority { get; set; }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string? ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the scopes, seperated by spaces.
        /// </summary>
        public string? Scope { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to require https or not.
        /// </summary>
        public bool RequireHttps { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to validate the issuer name or not.
        /// </summary>
        public bool ValidateIssuerName { get; set; } = true;

        /// <summary>
        /// Gets or sets a delegate for retrieving an <see cref="HttpClient"/>.
        /// </summary>
        public Func<IServiceProvider, HttpClient>? GetBackchannelHttpClient { get; set; }
    }
}
