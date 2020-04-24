// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Tokens
{
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;
    using IdentityModel.Client;

    /// <summary>
    /// Helper methods for getting tokens.
    /// </summary>
    public static class TokenHelpers
    {
        /// <summary>
        /// Transforms a TokenResponse into an <see cref="ITokenInfo"/>.
        /// </summary>
        /// <param name="response">The <see cref="TokenResponse"/>.</param>
        /// <returns>The ITokenInfo.</returns>
        public static ITokenInfo CreateTokenInfo(TokenResponse response)
        {
            if (response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return new TokenInfo
            {
                AccessToken = response.AccessToken,
                IdentityToken = response.IdentityToken,
                TokenType = response.TokenType,
                RefreshToken = response.RefreshToken,
                Expiration = DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn),
            };
        }

        /// <summary>
        /// Gets a token using the client_credentials grant.
        /// </summary>
        /// <param name="httpClient">The HttpClient to use.</param>
        /// <param name="options">The options for retrieving the token.</param>
        /// <returns>The TokenResponse.</returns>
        public static async Task<TokenResponse> GetClientCredentialsTokenAsync(HttpClient httpClient, ClientCredentialsTokenClientOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.ClientId == null)
            {
                throw new ArgumentOutOfRangeException(nameof(options), string.Format(CultureInfo.InvariantCulture, Properties.Resources.PropertyMustNotBeNullErrorMessage, nameof(options.ClientId)));
            }

            if (options.ClientSecret == null)
            {
                throw new ArgumentOutOfRangeException(nameof(options), string.Format(CultureInfo.InvariantCulture, Properties.Resources.PropertyMustNotBeNullErrorMessage, nameof(options.ClientSecret)));
            }

            if (options.Authority == null)
            {
                throw new ArgumentOutOfRangeException(nameof(options), string.Format(CultureInfo.InvariantCulture, Properties.Resources.PropertyMustNotBeNullErrorMessage, nameof(options.Authority)));
            }

            return await GetClientCredentialsTokenAsync(httpClient, options.ClientId, options.ClientSecret, options.Scope, options.Authority, options.RequireHttps, options.ValidateIssuerName).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a token using the client_credentials grant.
        /// </summary>
        /// <param name="httpClient">The HttpClient to use.</param>
        /// <param name="clientId">The clientId.</param>
        /// <param name="clientSecret">The clientSecret.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="authority">The authority.</param>
        /// <returns>The TokenResponse.</returns>
        public static async Task<TokenResponse> GetClientCredentialsTokenAsync(HttpClient httpClient, string clientId, string clientSecret, string? scope, string authority)
        {
            DiscoveryDocumentResponse dr = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = authority,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = true,
                    ValidateIssuerName = true,
                },
            }).ConfigureAwait(false);

            if (dr.IsError)
            {
                throw dr.Exception;
            }

            TokenResponse response = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = dr.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope,
            }).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Gets a token using the client_credentials grant.
        /// </summary>
        /// <param name="httpClient">The HttpClient to use.</param>
        /// <param name="clientId">The clientId.</param>
        /// <param name="clientSecret">The clientSecret.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="authority">The authority.</param>
        /// <param name="requireHttps">The requireHttps flag.</param>
        /// <param name="validateIssuer">The validateIssuer flag.</param>
        /// <returns>The TokenResponse.</returns>
        public static async Task<TokenResponse> GetClientCredentialsTokenAsync(HttpClient httpClient, string clientId, string clientSecret, string? scope, string authority, bool requireHttps, bool validateIssuer)
        {
            DiscoveryDocumentResponse dr = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = authority,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = requireHttps,
                    ValidateIssuerName = validateIssuer,
                },
            }).ConfigureAwait(false);

            if (dr.IsError)
            {
                throw dr.Exception;
            }

            TokenResponse response = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = dr.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope,
            }).ConfigureAwait(false);

            return response;
        }
    }
}
