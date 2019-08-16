using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Activators.Creators
{
    public class ClassicCreator:Creator
    {
        object[] _parameters;

        public ClassicCreator(object[] parameters)
        {
            _parameters = parameters;
        }

        public override object Create(Type typeOfInstance)
        {
            var typeOfInstanceConstructors = typeOfInstance.GetConstructors();

            // должен быть только один DI-конструктор, чтобы исключить многословность c#
            if (typeOfInstanceConstructors.Length != 1)
            {
                throw new ApplicationException($"Класс {typeOfInstance.FullName} имеет более одного конструктора.");
            }

            var typeOfInstanceConstructor = typeOfInstanceConstructors.First();

            if (typeOfInstanceConstructor.GetParameters().Length != _parameters.Length)
            {
                throw new ApplicationException($"Количество параметров в конструкторе класса {typeOfInstance.FullName} не соответствует кол-ву параметров, переданных для инициализации");
            }


            // вызываем конструктор - создаем экземпляр
            var instance = typeOfInstanceConstructor.Invoke(_parameters);

            return instance;
        }
    }
}
