using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCore.AsyncInitialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using Quartz;
using Waves.Core.Base.Attributes;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Extensions;
using Waves.Core.Services;

namespace Waves.Core.Base;

/// <summary>
/// Abstract waves framework startup class.
/// </summary>
public abstract class WavesStartup : IWavesStartup
{
    /// <summary>
    /// Gets jobs config key.
    /// </summary>
    private const string JobsParameterKey = "Jobs";

    /// <inheritdoc />
    public IConfiguration Configuration { get; private set; }

    /// <inheritdoc />
    public void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // set configuration.
        Configuration = context.Configuration;

        // register plugins.
        RegisterPlugins(services);

        // add logging.
        services.AddLogging();

        // run virtual method for registering additional services.
        ConfigureAdditionalServices(services);
    }

    /// <summary>
    /// Configures additional services.
    /// </summary>
    /// <param name="services">Services collection.</param>
    protected virtual void ConfigureAdditionalServices(IServiceCollection services)
    {
        // TODO: add your services here.
    }

    /// <summary>
    /// Registers plugins with type loader service.
    /// </summary>
    /// <param name="services">Service collection.</param>
    private async void RegisterPlugins(IServiceCollection services)
    {
        var typeLoader = new WavesTypeLoaderService<WavesPluginAttribute>();
        await typeLoader.UpdateTypesAsync();
        var types = typeLoader.Types;

        services.AddQuartz(q =>
        {
            var jobConfigurations = Configuration.GetSection(JobsParameterKey).Get<List<WavesJobConfiguration>>();

            q.UseMicrosoftDependencyInjectionJobFactory();
            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();

            var jobs = types.Where(x => x.Key.GetInterfaces().Contains(typeof(IWavesJob))).Select(x => x.Key);

            foreach (var job in jobs)
            {
                q.InitializeJob(job, jobConfigurations.SingleOrDefault(x => x.Name.Equals(job.GetFriendlyName())), services);
            }
        });

        services.AddQuartzHostedService(
            q => q.WaitForJobsToComplete = true);

        foreach (var type in types)
        {
            var attribute = type.Value;

            if (type.Key.GetInterfaces().Contains(typeof(IWavesJob)))
            {
                continue;
            }

            if (type.Key.GetInterfaces().Contains(typeof(IWavesPluginInitializable)))
            {
                services.AddAsyncInitializer(type.Key);
                continue;
            }

            switch (attribute.Lifetime)
            {
                case WavesLifetimeType.Transient:
                    services.AddTransient(attribute.Type, type.Key);
                    break;
                case WavesLifetimeType.Scoped:
                    services.AddScoped(attribute.Type, type.Key);
                    break;
                case WavesLifetimeType.Singleton:
                    services.AddSingleton(attribute.Type, type.Key);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
