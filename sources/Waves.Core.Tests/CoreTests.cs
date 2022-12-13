using System.Diagnostics;
using Autofac;
using Microsoft.Extensions.Logging;
using Waves.Core.Services.Interfaces;

namespace Waves.Core.Tests;

/// <summary>
/// <see cref="WavesCore"/> tests.
/// </summary>
public class CoreTests
{
    /// <summary>
    /// Setup.
    /// </summary>
    [SetUp]
    public void Setup()
    {
    }

    /// <summary>
    /// Tests that <see cref="WavesCore"/> starts successfully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WavesCore_StartCore_StartsSuccessfully()
    {
        var core = new WavesCore();
        await core.StartAsync();
        Assert.Pass();
    }

    /// <summary>
    /// Tests that <see cref="WavesCore"/> container builds successfully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WavesCore_BuildContainer_BuildsSuccessfully()
    {
        var core = new WavesCore();
        await core.StartAsync();
        var container = await core.BuildContainerAsync();
        Assert.That(container, Is.Not.Null);
    }

    /// <summary>
    /// Tests that <see cref="WavesCore"/> services provider resolves successfully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WavesCore_ResolveServiceProvider_ResolvesSuccessfully()
    {
        var core = new WavesCore();
        await core.StartAsync();
        var container = await core.BuildContainerAsync();
        var serviceProvider = container.Resolve<IWavesServiceProvider>();
        Assert.That(serviceProvider, Is.Not.Null);
    }

    /// <summary>
    /// Tests that <see cref="WavesCore"/> logging resolves successfully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WavesCore_ResolveLogging_ResolvesSuccessfully()
    {
        var core = new WavesCore();
        await core.StartAsync();
        var container = await core.BuildContainerAsync();
        var serviceProvider = container.Resolve<IWavesServiceProvider>();
        var logger = await serviceProvider.GetInstanceAsync<ILogger<WavesCore>>();
        Assert.That(logger, Is.Not.Null);
    }
}
