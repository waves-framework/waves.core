using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Fluid.Core.Interfaces;

namespace Fluid.Core.Base
{
    public class EntryPoint : Object, IEntryPoint
    {
        private Guid _id = Guid.NewGuid();

        private bool _isProperty;
        private string _name;
        private object _value = new object();

        private IModule _parent;

        /// <summary>
        ///     Создает новый экземпляр
        /// </summary>
        /// <param name="parent"></param>
        public EntryPoint(IModule parent)
        {
            Parent = parent;
        }

        /// <summary>
        ///     Определяет является ли точкой для свойства модуля
        /// </summary>
        public bool IsProperty
        {
            get => _isProperty;
            set
            {
                if (value == _isProperty) return;
                _isProperty = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Имя точки
        /// </summary>
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
        ///     Значение точки
        /// </summary>
        public object Value
        {
            get => _value;
            set
            {
                if (Equals(value, _value)) return;
                _value = value;
                OnPropertyChanged();
                OnDataReceived(_value);
            }
        }

        /// <summary>
        ///     Идентификатор точки
        /// </summary>
        public override Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        ///     Ссылка на родительский модуль точки
        /// </summary>
        public IModule Parent
        {
            get => _parent;
            internal set
            {
                if (Equals(value, _parent)) return;
                _parent = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Событие отправки системного сообщения
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<IMessage> MessageReceived;

        /// <summary>
        ///     Событие приема данных на точку
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<object> DataReceived;

        /// <summary>
        ///     Dispose
        /// </summary>
        public void Dispose()
        {
            Parent = null;
            Value = null;
        }

        /// <summary>
        ///     Уведомление об отправке системного сообщения.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMessageReceived(IMessage e)
        {
            MessageReceived?.Invoke(this, e);
        }

        /// <summary>
        ///     Уведомление о приеме данных.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDataReceived(object e)
        {
            DataReceived?.Invoke(this, e);
        }
    }
}