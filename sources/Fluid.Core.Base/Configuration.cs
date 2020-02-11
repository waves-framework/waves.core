using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Fluid.Core.Interfaces;

namespace Fluid.Core.Base
{
    [Serializable]
    public class Configuration : Object, IConfiguration
    {
        private string _name = string.Empty;

        private ICollection<IProperty> _properties = new List<IProperty>();

        /// <summary>
        ///     Новый экземпляр конфигурации.
        /// </summary>
        public Configuration()
        {
        }

        /// <summary>
        ///     Новый экземпляр конфигурации.
        /// </summary>
        /// <param name="name"></param>
        public Configuration(string name)
        {
            Name = name;
        }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public sealed override string Name { get; set; }

        /// <summary>
        ///     Набор свойств
        /// </summary>
        public ICollection<IProperty> Properties
        {
            get => _properties;
            set
            {
                if (Equals(value, _properties)) return;
                _properties = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Событие отправки системного сообщения
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<IMessage> MessageReceived;

        /// <summary>
        ///     Добавление свойства
        /// </summary>
        /// <param name="property"></param>
        public void AddProperty(IProperty property)
        {
            if (string.IsNullOrEmpty(property.Name) ||
                property.Value == null)
                throw new Exception("При добавлении свойства заданы неверные входные данные!");

            if (property.Value.GetType().IsSerializable)
            {
                // Проверяем есть ли свойство с таким же именем
                foreach (var p in Properties)
                    if (p.Name == property.Name)
                        throw new Exception("Свойство с таким именем уже существует! (" + property.Name + ")");

                Properties.Add(property);
            }
            else
            {
                throw new Exception("Заданное свойство не поддерживает сериализацию! " + "(" + property.Name + ")");
            }
        }

        /// <summary>
        ///     Получение свойства
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetPropertyValue(string name)
        {
            foreach (var property in Properties)
            {
                if (property.Name != name) continue;
                return property.Value;
            }

            throw new Exception("Свойства с таким именем не существует! " + "(" + name + ")");
        }

        /// <summary>
        ///     Обновление значения свойства
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetPropertyValue(string name, object value)
        {
            foreach (var property in Properties)
            {
                if (property.IsReadOnly) continue;
                if (property.Name != name) continue;

                property.Value = value;
                return;
            }

            throw new Exception("Свойства с таким именем не существует! " + "(" + name + ")");
        }

        /// <summary>
        ///     Удаление свойства
        /// </summary>
        /// <param name="name"></param>
        public void RemoveProperty(string name)
        {
            foreach (var property in Properties)
            {
                if (property.Name != name) continue;
                Properties.Remove(property);
                return;
            }

            throw new Exception("Свойства с таким именем не существует! " + "(" + name + ")");
        }

        /// <inheritdoc />
        public bool Contains(string name)
        {
            foreach (var property in Properties)
                if (property.Name.Equals(name))
                    return true;

            return false;
        }

        /// <summary>
        ///     Получение Hash-кода объекта
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = 2;

            foreach (var property in Properties)
                hashCode = hashCode * 31 + property.GetHashCode();

            return hashCode;
        }

        /// <summary>
        ///     Клонирование
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var configuration = new Configuration();

            foreach (var property in Properties)
                configuration.Properties.Add((Property) property.Clone());

            return configuration;
        }

        /// <summary>
        ///     Уведомление об отправке системного сообщения.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMessageReceived(IMessage e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
}