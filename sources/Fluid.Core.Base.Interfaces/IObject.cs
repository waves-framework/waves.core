using System;

namespace Fluid.Core.Base.Interfaces
{
    public interface IObject : IObservableObject
    {
        /// <summary>
        /// Идентификатор объекта.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Наименование объекта.
        /// </summary>
        string Name { get; }
    }
}