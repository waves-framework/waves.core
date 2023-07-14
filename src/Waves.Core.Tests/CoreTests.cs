using System.Diagnostics;
using Autofac;
using Microsoft.Extensions.Logging;
using Waves.Core.Services.Interfaces;
using Waves.Core.Tests.Services;

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

    /// <summary>
    /// Tests that <see cref="WavesCore"/> test service resolves successfully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WavesCore_ResolveTestService_ResolvesSuccessfully()
    {
        var core = new WavesCore();
        await core.StartAsync();
        var container = await core.BuildContainerAsync();
        var serviceProvider = container.Resolve<IWavesServiceProvider>();
        var service = await serviceProvider.GetInstanceAsync<TestService>();
        Assert.That(service, Is.Not.Null);
    }

    /// <summary>
    /// Tests that <see cref="WavesCore"/> initializable test service resolves successfully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WavesCore_ResolveInitializableTestService_TestValueEqualsExpected()
    {
        var core = new WavesCore();
        await core.StartAsync();
        var container = await core.BuildContainerAsync();
        var serviceProvider = container.Resolve<IWavesServiceProvider>();
        var service = await serviceProvider.GetInstanceAsync<TestInitializableService>();
        Assert.That(service.TestValue, Is.EqualTo(50));
    }

    /// <summary>
    /// Tests that <see cref="WavesCore"/> initializable test service resolves successfully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WavesCore_ResolveInitializableTestService_TestValueNotEqualsZero()
    {
        var core = new WavesCore();
        await core.StartAsync();
        var container = await core.BuildContainerAsync();
        var serviceProvider = container.Resolve<IWavesServiceProvider>();
        var service = await serviceProvider.GetInstanceAsync<TestInitializableService>();
        Assert.That(service.TestValue, Is.Not.EqualTo(0));
    }

    /// <summary>
    /// Tests that <see cref="WavesCore"/> configurable test service resolves successfully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WavesCore_ResolveConfigurableTestService_TestValueEqualsExpected()
    {
        var core = new WavesCore();
        await core.StartAsync();
        var container = await core.BuildContainerAsync();
        var serviceProvider = container.Resolve<IWavesServiceProvider>();
        var service = await serviceProvider.GetInstanceAsync<TestConfigurableService>();
        Assert.That(service.TestValue, Is.EqualTo(25));
    }

    /// <summary>
    /// Tests that <see cref="WavesCore"/> configurable test service resolves successfully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WavesCore_ResolveConfigurableTestService_TestValueNotEqualsZero()
    {
        var core = new WavesCore();
        await core.StartAsync();
        var container = await core.BuildContainerAsync();
        var serviceProvider = container.Resolve<IWavesServiceProvider>();
        var service = await serviceProvider.GetInstanceAsync<TestConfigurableService>();
        Assert.That(service.TestValue, Is.Not.EqualTo(0));
    }
}
