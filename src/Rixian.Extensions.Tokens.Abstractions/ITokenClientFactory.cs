// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Tokens
{
    using Rixian.Extensions.Errors;

    /// <summary>
    /// A factory that can create ITokenClient instances.
    /// </summary>
    public interface ITokenClientFactory
    {
        /// <summary>
        /// Creates an instance of an ITokenClient using the given logical name.
        /// </summary>
        /// <param name="name">The logical name of the ITokenClient to create.</param>
        /// <returns>An instance of an ITokenClient.</returns>
        Result<ITokenClient> GetTokenClient(string name);
    }
}
