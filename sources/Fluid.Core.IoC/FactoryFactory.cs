using System;

namespace Fluid.Core.IoC
{
    /// <summary>
    ///     Factory of Factory.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    internal class FactoryFactory<T>
    {
        public Func<T> Create(SimpleContainer container)
        {
            return () => (T) container.GetInstance(typeof(T), null);
        }
    }
}