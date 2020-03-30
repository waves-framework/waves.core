using System;

namespace Fluid.Core.IoC
{
    internal class FactoryFactory<T>
    {
        public Func<T> Create(SimpleContainer container)
        {
            return () => (T) container.GetInstance(typeof(T), null);
        }
    }
}