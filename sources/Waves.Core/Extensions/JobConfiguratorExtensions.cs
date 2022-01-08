using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Waves.Core.Base;
using Waves.Core.Base.Enums;

namespace Waves.Core.Extensions
{
    /// <summary>
    /// Jobs configurator.
    /// </summary>
    public static class JobConfiguratorExtensions
    {
        /// <summary>
        /// Initializes job.
        /// </summary>
        /// <param name="quartzServices">Quartz services.</param>
        /// <param name="jobType">Job type.</param>
        /// <param name="jobConfiguration">Job configuration.</param>
        /// <param name="services">Service collection.</param>
        public static void InitializeJob(
            this IServiceCollectionQuartzConfigurator quartzServices,
            Type jobType,
            WavesJobConfiguration jobConfiguration,
            IServiceCollection services)
        {
            void SetupTrigger(ITriggerConfigurator trigger)
            {
                trigger.WithIdentity(jobConfiguration.Name);
                ConfigureJobOffset(trigger, jobConfiguration);
                ConfigureJobInterval(trigger, jobConfiguration);
            }

            GenericExtensions.InvokeStaticGenericMethod(typeof(ServiceCollectionExtensions), "ScheduleJob", jobType, new object[] { quartzServices, SetupTrigger, null });
            services.AddTransient(jobType);
        }

        /// <summary>
        /// Initializes job.
        /// </summary>
        /// <param name="quartzServices">Quartz services.</param>
        /// <param name="jobConfiguration">Job configuration.</param>
        /// <param name="services">Service collection.</param>
        /// <typeparam name="T">Type of job.</typeparam>
        public static void InitializeJob<T>(
            this IServiceCollectionQuartzConfigurator quartzServices,
            WavesJobConfiguration jobConfiguration,
            IServiceCollection services)
            where T : class, IJob
        {
            void SetupTrigger(ITriggerConfigurator trigger)
            {
                trigger.WithIdentity(jobConfiguration.Name);
                ConfigureJobOffset(trigger, jobConfiguration);
                ConfigureJobInterval(trigger, jobConfiguration);
            }

            quartzServices.ScheduleJob<T>(SetupTrigger);
            services.AddTransient<T>();
        }

        /// <summary>
        /// Configures job execute offset.
        /// </summary>
        /// <param name="triggerConfigurator">Trigger configurator.</param>
        /// <param name="jobConfiguration">Job configuration.</param>
        private static void ConfigureJobOffset(ITriggerConfigurator triggerConfigurator, WavesJobConfiguration jobConfiguration)
        {
            switch (jobConfiguration.OffsetUnit)
            {
                case WavesJobIntervalUnit.Second:
                    triggerConfigurator.StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(jobConfiguration.Offset)));
                    break;
                case WavesJobIntervalUnit.Minute:
                    triggerConfigurator.StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddMinutes(jobConfiguration.Offset)));
                    break;
                case WavesJobIntervalUnit.Hour:
                    triggerConfigurator.StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddHours(jobConfiguration.Offset)));
                    break;
                case WavesJobIntervalUnit.Day:
                    triggerConfigurator.StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddDays(jobConfiguration.Offset)));
                    break;
                case WavesJobIntervalUnit.Month:
                    triggerConfigurator.StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddMonths(jobConfiguration.Offset)));
                    break;
                case WavesJobIntervalUnit.Year:
                    triggerConfigurator.StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddYears(jobConfiguration.Offset)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Configures job execute interval.
        /// </summary>
        /// <param name="triggerConfigurator">Trigger configurator.</param>
        /// <param name="jobConfiguration">Job configuration.</param>
        private static void ConfigureJobInterval(ITriggerConfigurator triggerConfigurator, WavesJobConfiguration jobConfiguration)
        {
            triggerConfigurator.WithSimpleSchedule(x => ConfigureJobInterval(x, jobConfiguration));
        }

        /// <summary>
        /// Configures job execute interval.
        /// </summary>
        /// <param name="builder">Schedule builder.</param>
        /// <param name="jobConfiguration">Job configuration.</param>
        private static void ConfigureJobInterval(SimpleScheduleBuilder builder, WavesJobConfiguration jobConfiguration)
        {
            switch (jobConfiguration.IntervalUnit)
            {
                case WavesJobIntervalUnit.Second:
                    builder.WithIntervalInSeconds(jobConfiguration.Interval);
                    break;
                case WavesJobIntervalUnit.Minute:
                    builder.WithIntervalInMinutes(jobConfiguration.Interval);
                    break;
                case WavesJobIntervalUnit.Hour:
                    builder.WithIntervalInHours(jobConfiguration.Interval);
                    break;
                case WavesJobIntervalUnit.Day:
                    builder.WithInterval(TimeSpan.FromDays(jobConfiguration.Interval));
                    break;
                case WavesJobIntervalUnit.Month:
                    builder.WithInterval(TimeSpan.FromDays(30 * jobConfiguration.Interval));
                    break;
                case WavesJobIntervalUnit.Year:
                    builder.WithInterval(TimeSpan.FromDays(365 * jobConfiguration.Interval));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (jobConfiguration.RepeatForever)
            {
                builder.RepeatForever();
            }
        }
    }
}
