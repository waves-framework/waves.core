using System;
using System.Collections.Generic;
using System.Text;

namespace Fluid.Core.IoC
{
    class ContainerEntry : List<Func<SimpleContainer, object>>
    {
        public string Key;
        public Type Service;
    }
}
