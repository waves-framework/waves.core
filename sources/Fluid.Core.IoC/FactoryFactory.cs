using System;
using System.Collections.Generic;
using System.Text;

namespace Fluid.Core.IoC
{
    class FactoryFactory<T>
    {
        public Func<T> Create(SimpleContainer container)
        {
            return () => (T)container.GetInstance(typeof(T), null);
        }
    }
}
