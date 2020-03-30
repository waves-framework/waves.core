using System;
using System.Collections.Generic;

namespace Fluid.Core.IoC
{
    internal class ContainerEntry : List<Func<SimpleContainer, object>>
    {
        public string Key;
        public Type Service;
    }
}