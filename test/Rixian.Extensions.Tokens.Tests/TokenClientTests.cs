// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Rixian.Extensions.Errors;
using Rixian.Extensions.Tokens;
using Xunit;

public class TokenClientTests
{
    private readonly ITokenClientFactory tokenClientFactory;

    public TokenClientTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddHttpClient("tls12")
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                SslProtocols = SslProtocols.Tls12,
            });

        serviceCollection
            .AddClientCredentialsTokenClient(
                new ClientCredentialsTokenClientOptions
                {
                    ClientId = "REPLACE_ME",
                    ClientSecret = "REPLACE_ME",
                    Authority = "REPLACE_ME",
                    Scope = "REPLACE_ME",
                    RequireHttps = false,
                    ValidateIssuerName = false,
                })
            .UseHttpClientForBackchannel("tls12");

        IServiceProvider services = serviceCollection.BuildServiceProvider();

        this.tokenClientFactory = services.GetRequiredService<ITokenClientFactory>();
    }

    [Fact]
    public void TokenClient_Instantiate_Success()
    {
        Result<ITokenClient> tokenClientResult = this.tokenClientFactory.GetTokenClient();
        ITokenClient tokenClient = tokenClientResult.Value;

        tokenClient.Should().NotBeNull();
    }

    [Fact]
    [Trait("TestCategory", "FailsInCloudTest")]
    public async Task TokenRetrievalWorks()
    {
        Result<ITokenClient> tokenClientResult = this.tokenClientFactory.GetTokenClient();
        ITokenClient tokenClient = tokenClientResult.Value;
        Result<ITokenInfo> token = await tokenClient.GetTokenAsync().ConfigureAwait(false);
        token.IsSuccess.Should().BeTrue();
        token.Value.Should().NotBeNull();
        token.Value.AccessToken.Should().NotBeNull();
    }

    [Fact]
    [Trait("TestCategory", "FailsInCloudTest")]
    public async Task MultipleCallSameTokenTest()
    {
        Result<ITokenClient> tokenClientResult = this.tokenClientFactory.GetTokenClient();
        ITokenClient tokenClient = tokenClientResult.Value;
        Result<ITokenInfo> token1 = await tokenClient.GetTokenAsync().ConfigureAwait(false);
        Result<ITokenInfo> token2 = await tokenClient.GetTokenAsync().ConfigureAwait(false);

        token1.IsSuccess.Should().BeTrue();
        token1.Value.Should().NotBeNull();
        token1.Value.AccessToken.Should().NotBeNull();

        token2.IsSuccess.Should().BeTrue();
        token2.Value.Should().NotBeNull();
        token2.Value.AccessToken.Should().NotBeNull();
        token1.Value.AccessToken.Should().Be(token2.Value.AccessToken);
        token1.Value.Should().Be(token2.Value);
    }

    /// <summary>
    /// Based on this StackOverflow answer:
    /// https://stackoverflow.com/a/9468889
    ///
    ///
    /// Proving that something is thread safe is tricky - probably halting-problem hard.
    /// You can show that a race condition is easy to produce, or that it is hard to produce.
    /// But not producing a race condition doesn't mean it isn't there.
    ///
    /// But: my usual approach here (if I have reason to think a bit of code that should be thread-safe, isn't) is to
    /// spin up a lot of threads waiting behind a single ManualResetEvent. The last thread to get to the gate
    /// (using interlocked to count) is responsible for opening the gate so that all the threads hit the system at the same time (and already exist).
    /// Then they do the work and check for sane exit conditions. Then I repeat this process a large number of times.
    /// This is usually sufficient to reproduce a suspected thread-race, and show that it moves from "obviously broken"
    /// to "not broken in an obvious way" (which is crucially different to "not broken").
    ///
    /// Also note: most code does not have to be thread-safe.
    ///
    /// </summary>
    /// <param name="count">Number of tasks to run.</param>
    /// <returns>An awaitable task.</returns>
    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(10000)]
    [InlineData(100000)]
    [InlineData(1000000)]
    [Trait("TestCategory", "FailsInCloudTest")]
    public async Task ThreadSafetyTest(int count)
    {
        var tasks = new List<Task<Result<ITokenInfo>>>();
        using (var resetEvent = new ManualResetEvent(false))
        {
            Result<ITokenClient> tokenClientResult = this.tokenClientFactory.GetTokenClient();

            async Task<Result<ITokenInfo>> GetTokenAsync()
            {
                resetEvent.WaitOne();
                return await tokenClientResult.Value.GetTokenAsync().ConfigureAwait(false);
            }

            for (int i = 0; i < count; i++)
            {
                tasks.Add(Task.Run(() => GetTokenAsync()));
            }

            resetEvent.Set();

            Result<ITokenInfo>[] results = await Task.WhenAll(tasks).ConfigureAwait(false);
            results.All(r => r.IsSuccess).Should().BeTrue();
            results.Select(ti => ti.Value.AccessToken).Distinct().Should().HaveCount(1);
        }
    }
}
