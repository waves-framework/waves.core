using System;
using Autofac;

namespace Waves.Core.Extensions;

/// <summary>
/// Extensions for Autofac container builder.
/// </summary>
internal static class ContainerBuilderExtensions
{
    /// <summary>
    /// Registers transient instance.
    /// </summary>
    /// <param name="builder">Container builder.</param>
    /// <param name="currentType">Current of instance.</param>
    /// <param name="registerType">With which type instance will be registered.</param>
    /// <param name="key">Instance key.</param>
    internal static void RegisterTransient(this ContainerBuilder builder, Type currentType, Type registerType, object key = null)
    {
        if (key == null)
        {
            builder.RegisterType(currentType).As(registerType);
        }
        else
        {
            builder.RegisterType(currentType).As(registerType).Keyed(key, registerType);
        }
    }
}
