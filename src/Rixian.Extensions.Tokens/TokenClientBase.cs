// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Tokens
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Nito.AsyncEx;
    using Rixian.Extensions.Errors;
    using static Rixian.Extensions.Errors.Prelude;

    /// <summary>
    /// Token Client used for the Client Credentials grant type.
    /// </summary>
    public abstract class TokenClientBase : ITokenClient
    {
        private readonly object gate = new object();
        private readonly object getTokenGate = new object();

        private ITokenInfo? token;
        private DateTimeOffset expiration;

        private AsyncLazy<Result<ITokenInfo>>? gettingTokenTask = null;

        /// <inheritdoc/>
        public Task<Result<ITokenInfo>> GetTokenAsync(bool forceRefresh)
        {
            lock (this.gate)
            {
                if (forceRefresh)
                {
                    this.token = null;
                }

                if (this.token != null && DateTimeOffset.UtcNow < this.expiration)
                {
                    return Task.FromResult(Result(this.token));
                }

                lock (this.getTokenGate)
                {
                    if (this.gettingTokenTask == null)
                    {
                        this.gettingTokenTask = new AsyncLazy<Result<ITokenInfo>>(async () =>
                        {
                            try
                            {
                                Result<ITokenInfo> tokenResult = await this.GetTokenCoreAsync().ConfigureAwait(false);
                                if (tokenResult.IsSuccess)
                                {
                                    this.token = tokenResult.Value;
                                    this.expiration = this.token.Expiration.AddMinutes(-5); // Give ourselves a 5 minute buffer
                                    lock (this.getTokenGate)
                                    {
                                        this.gettingTokenTask = null;
                                    }

                                    return Result(this.token);
                                }
                                else
                                {
                                    this.token = null;
                                    return tokenResult;
                                }
                            }
                            catch
                            {
                                this.token = null;
                                this.expiration = DateTimeOffset.MinValue;
                                throw;
                            }
                        });
                    }

                    return this.gettingTokenTask.Task;
                }
            }
        }

        /// <summary>
        /// The core implementation to retreive a token.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<Result<ITokenInfo>> GetTokenCoreAsync();
    }
}
