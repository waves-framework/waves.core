using System;
using Autofac;
using Autofac.Core;

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
            builder.RegisterType(currentType).As(registerType).OnActivated(OnActivated);
        }
        else
        {
            builder.RegisterType(currentType).As(registerType).Keyed(key, registerType).OnActivated(OnActivated);
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
            builder.RegisterInstance(obj).As(registerType).OnActivated(OnActivated);
        }
        else
        {
            builder.RegisterInstance(obj).As(registerType).Keyed(key, registerType).OnActivated(OnActivated);
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
            builder.RegisterType(currentType).As(registerType).InstancePerLifetimeScope().OnActivated(OnActivated);
        }
        else
        {
            builder.RegisterType(currentType).As(registerType).Keyed(key, registerType).InstancePerLifetimeScope().OnActivated(OnActivated);
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
            builder.RegisterInstance(obj).As(registerType).InstancePerLifetimeScope().OnActivated(OnActivated);
        }
        else
        {
            builder.RegisterInstance(obj).As(registerType).Keyed(key, registerType).InstancePerLifetimeScope().OnActivated(OnActivated);
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
            builder.RegisterType(currentType).As(registerType).SingleInstance().OnActivated(OnActivated);
        }
        else
        {
            builder.RegisterType(currentType).As(registerType).Keyed(key, registerType).SingleInstance().OnActivated(OnActivated);
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
            builder.RegisterInstance(obj).As(registerType).SingleInstance().OnActivated(OnActivated);
        }
        else
        {
            builder.RegisterInstance(obj).As(registerType).Keyed(key, registerType).SingleInstance().OnActivated(OnActivated);
        }
    }

    /// <summary>
    /// Callback when activated.
    /// </summary>
    /// <param name="obj">Object.</param>
    private static void OnActivated(IActivatedEventArgs<object> obj)
    {
        var instance = obj.Instance;
        instance.CheckInitializable();
    }
}
