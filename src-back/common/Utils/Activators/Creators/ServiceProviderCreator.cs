using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Activators.Creators
{
    /// <summary>
    /// Инициализация параметров констроктора из DI
    /// </summary>
    public class ServiceProviderCreator : SpCreator
    {
        IServiceProvider _provider;

        public ServiceProviderCreator(IServiceProvider provider)
        {
            _provider = provider;
        }

        public override object Create(Type typeOfInstance) 
        {
            var typeOfInstanceConstructors = typeOfInstance.GetConstructors();

            // должен быть только один DI-конструктор, чтобы исключить многословность c#
            if (typeOfInstanceConstructors.Length != 1)
            {
                throw new ApplicationException($"Класс {typeOfInstance.FullName} имеет более одного конструктора. В текущей архитектуре может быть только один DI-конструктор");
            }

            var typeOfInstanceConstructor = typeOfInstanceConstructors.First();

            // инициализируем параметры конструктора из DI-сервисов
            var parameters = typeOfInstanceConstructor.GetParameters()
                .Select(p => {
                    // все типы параметров должны быть прописаны в DI-сервисах (никаких int a, string b и т.п. "левых" типов)
                    var paramInstance = _provider.GetService(p.ParameterType);
                    if (paramInstance == null)
                    {
                        throw new ApplicationException($"Тип параметра конструктора `{p.ParameterType.Name} {p.Name}` класса `{typeOfInstance.FullName}` не найден в сервисах DI. Возможно тип `{p.ParameterType.Name}` не добавлен в ServiceCollection на инициализации приложения в классе Startup.ConfigureServices");
                    }
                    return paramInstance;
                })
                .ToArray();

            // вызываем конструктор - создаем экземпляр
            var instance = typeOfInstanceConstructor.Invoke(parameters);

            return instance;
        }
    }
}
