using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utils.Activators.Creators;

namespace Utils.Activators
{
    /// <summary>
    /// Создание экземпляра класса
    /// </summary>
    public class InstanceCreator // TODO: переделать. Экзепляры используются по назначению из основного класса(где создается). Их время жизни = в.ж основного класса. Должен учитываться интерфейс IDisposable
    {
        /// <summary>
        /// Создает классы, конструктор которых принимает параметры явно
        /// </summary>
        public static TCreator Use<TCreator>(object[] parameters) where TCreator : ClassicCreator
        {
            return new ClassicCreator(parameters) as TCreator;
        }

        /// <summary>
        /// Создает классы с пустым конструктором
        /// </summary>
        public static TCreator Use<TCreator>() where TCreator: ClassicCreator 
        {
            return Use<TCreator>(new object[] { }) as TCreator;
        }

        /// <summary>
        /// Создает классы инициализация которых идет через DI
        /// </summary>
        public static TCreator Use<TCreator>(IServiceProvider provider) where TCreator : SpCreator
        {
            if (typeof(TCreator) == typeof(ServiceProviderCreator))
            {
                // создаем экземпляр ServiceProviderCreator, через ClassicCreator

                return Use<ClassicCreator>(new object[] { provider })
                .Create<ServiceProviderCreator>() as TCreator;
            }

            return new ServiceProviderPropertyCreator(provider, new object[] { }) as TCreator;
        }


        /// <summary>
        /// Создает классы, конструктор которых принимает параметры явно, а свойства инициализируеются через DI
        /// </summary>
        public static TCreator Use<TCreator>(IServiceProvider provider, object[] parameters) where TCreator : ServiceProviderPropertyCreator
        {
            return new ServiceProviderPropertyCreator(provider, parameters) as TCreator;
        }
    }
}
