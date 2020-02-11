using System;

namespace Fluid.Core.Interfaces
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