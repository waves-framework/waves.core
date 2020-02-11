using System;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    [Serializable]
    public class Property : Object, IProperty
    {
        private bool _isReadOnly;
        private string _name;
        private object _value;

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="isReadOnly"></param>
        /// <exception cref="Exception"></exception>
        public Property(string name, object value, bool isReadOnly)
        {
            IsReadOnly = isReadOnly;
            Name = name;
            Value = value;
        }

        /// <inheritdoc />
        public sealed override string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Является ли свойство доступным только для чтения
        /// </summary>
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                if (value == _isReadOnly) return;
                _isReadOnly = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        ///     Значение
        /// </summary>
        public object Value
        {
            get => _value;
            set
            {
                if (Equals(value, _value)) return;
                _value = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Получение Hash-кода объекта
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = 1;

            hashCode = (hashCode * 397) ^ Name.GetHashCode();
            hashCode = (hashCode * 397) ^ Value.GetHashCode();

            return hashCode;
        }

        /// <summary>
        ///     Клонирование
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Property(Name, Value, IsReadOnly);
        }
    }
}