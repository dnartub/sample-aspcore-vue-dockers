using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Activators.Creators
{
    /// <summary>
    /// Передача параметров в конструктор
    /// ИНициализация DI свойств
    /// 
    /// </summary>
    public class ServiceProviderPropertyCreator: SpCreator
    {
        IServiceProvider _provider;
        object[] _parameters = new object[] { };

        public ServiceProviderPropertyCreator(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ServiceProviderPropertyCreator(IServiceProvider provider, object[] parameters)
        {
            _provider = provider;
            _parameters = parameters;
        }

        public override object Create(Type typeOfInstance)
        {
            // в конструкторе класса обычные параметры - не из DI
            var instance = new ClassicCreator(_parameters)
                .Create(typeOfInstance);

            // иницилизируем  из контекста DIs все public свойств класса с аттрибутом [DiService]
            var diProperties = typeOfInstance.GetProperties()
                .Where(p => p.CanWrite && p.SetMethod.IsPublic) // все паблик свойства доступные для определения
                .Where(p => p.CustomAttributes.Any(a=>a.AttributeType == typeof(DiServiceAttribute)))
                .ToList();

            foreach (var diProperty in diProperties)
            {
                // берем экземпляр из контекста DI
                var diInstance = _provider.GetService(diProperty.PropertyType);
                // устанвливаем свойство экземпляра класса
                diProperty.SetValue(instance, diInstance);
            }

            return instance;
        }
    }
}
