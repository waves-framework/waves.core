using System;
using Autofac;

namespace Waves.Core.Extensions;

/// <summary>
/// Extensions for Autofac container builder.
/// </summary>
internal static class ContainerBuilderExtensions
{
    /// <summary>
    /// Registers transient type.
    /// </summary>
    /// <param name="builder">Container builder.</param>
    /// <param name="currentType">Current type.</param>
    /// <param name="registerType">With which type instance will be registered.</param>
    /// <param name="key">Instance key.</param>
    internal static void RegisterTransientType(this ContainerBuilder builder, Type currentType, Type registerType, object key = null)
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

    /// <summary>
    /// Registers transient instance.
    /// </summary>
    /// <param name="builder">Container builder.</param>
    /// <param name="obj">Current object.</param>
    /// <param name="registerType">With which type instance will be registered.</param>
    /// <param name="key">Instance key.</param>
    internal static void RegisterTransientInstance(this ContainerBuilder builder, object obj, Type registerType, object key = null)
    {
        if (key == null)
        {
            builder.RegisterInstance(obj).As(registerType);
        }
        else
        {
            builder.RegisterInstance(obj).As(registerType).Keyed(key, registerType);
        }
    }

    /// <summary>
    /// Registers scoped type.
    /// </summary>
    /// <param name="builder">Container builder.</param>
    /// <param name="currentType">Current type.</param>
    /// <param name="registerType">With which type instance will be registered.</param>
    /// <param name="key">Instance key.</param>
    internal static void RegisterScopedType(this ContainerBuilder builder, Type currentType, Type registerType, object key = null)
    {
        if (key == null)
        {
            builder.RegisterType(currentType).As(registerType).InstancePerLifetimeScope();
        }
        else
        {
            builder.RegisterType(currentType).As(registerType).Keyed(key, registerType).InstancePerLifetimeScope();
        }
    }

    /// <summary>
    /// Registers scoped instance.
    /// </summary>
    /// <param name="builder">Container builder.</param>
    /// <param name="obj">Current object.</param>
    /// <param name="registerType">With which type instance will be registered.</param>
    /// <param name="key">Instance key.</param>
    internal static void RegisterScopedInstance(this ContainerBuilder builder, object obj, Type registerType, object key = null)
    {
        if (key == null)
        {
            builder.RegisterInstance(obj).As(registerType).InstancePerLifetimeScope();
        }
        else
        {
            builder.RegisterInstance(obj).As(registerType).Keyed(key, registerType).InstancePerLifetimeScope();
        }
    }

    /// <summary>
    /// Registers single type.
    /// </summary>
    /// <param name="builder">Container builder.</param>
    /// <param name="currentType">Current type.</param>
    /// <param name="registerType">With which type instance will be registered.</param>
    /// <param name="key">Instance key.</param>
    internal static void RegisterSingletonType(this ContainerBuilder builder, Type currentType, Type registerType, object key = null)
    {
        if (key == null)
        {
            builder.RegisterType(currentType).As(registerType).SingleInstance();
        }
        else
        {
            builder.RegisterType(currentType).As(registerType).Keyed(key, registerType).SingleInstance();
        }
    }

    /// <summary>
    /// Registers single instance.
    /// </summary>
    /// <param name="builder">Container builder.</param>
    /// <param name="obj">Current object.</param>
    /// <param name="registerType">With which type instance will be registered.</param>
    /// <param name="key">Instance key.</param>
    internal static void RegisterSingletonInstance(this ContainerBuilder builder, object obj, Type registerType, object key = null)
    {
        if (key == null)
        {
            builder.RegisterInstance(obj).As(registerType).SingleInstance();
        }
        else
        {
            builder.RegisterInstance(obj).As(registerType).Keyed(key, registerType).SingleInstance();
        }
    }
}
