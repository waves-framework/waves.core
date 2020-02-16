using System;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    [Serializable]
    public class Connection : Object, IConnection
    {
        private IEntryPoint _input;
        private IEntryPoint _output;
        private string _name;

        /// <summary>
        ///     Создает новую копию соединения
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public Connection(IEntryPoint input, IEntryPoint output)
        {
            Initialize(input, output);
        }

        /// <summary>
        ///     Идентификатор соединения
        /// </summary>
        public override Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public override string Name
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
        ///     Точка входа в соединение
        /// </summary>
        public IEntryPoint Input
        {
            get => _input;
            set
            {
                if (Equals(value, _input)) return;
                _input = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Точка выхода из соединения
        /// </summary>
        public IEntryPoint Output
        {
            get => _output;
            set
            {
                if (Equals(value, _output)) return;
                _output = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Получение Hash-кода объекта
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = 4;

            return hashCode;
        }

        /// <summary>
        ///     Клонирование
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Dispose
        /// </summary>
        public void Dispose()
        {
            Input = null;
            Output = null;
        }

        /// <summary>
        ///     Инициализация
        /// </summary>
        public void Initialize(IEntryPoint input, IEntryPoint output)
        {
            if (input == null || output == null) return;

            Input = input;
            Output = output;
        }
    }
}