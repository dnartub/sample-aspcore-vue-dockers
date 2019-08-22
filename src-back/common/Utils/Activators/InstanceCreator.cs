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
    public class InstanceCreator:IDisposable 
    {
        public static InstanceCreator GetContext()
        {
            return new InstanceCreator();
        }

        private List<object> _createdInstances = new List<object>();


        /// <summary>
        /// Создает классы, конструктор которых принимает параметры явно
        /// </summary>
        public TInstance Create<TCreator, TInstance>(object[] parameters) where TCreator : ClassicCreator where TInstance: class
        {
            return Create<TCreator>(typeof(TInstance), parameters) as TInstance;
        }

        /// <summary>
        /// Создает классы, конструктор которых принимает параметры явно
        /// </summary>
        public object Create<TCreator>(Type instanceType, object[] parameters) where TCreator : ClassicCreator 
        {
            var creator = new ClassicCreator(parameters) as TCreator;
            _createdInstances.Add(creator);

            var result = creator.Create(instanceType);
            _createdInstances.Add(result);
            return result;
        }


        /// <summary>
        /// Создает классы с пустым конструктором
        /// </summary>
        public TInstance Create<TCreator, TInstance>() where TCreator: ClassicCreator where TInstance : class
        {
            return Create<TCreator, TInstance>(new object[] { });
        }

        /// <summary>
        /// Создает классы с пустым конструктором
        /// </summary>
        public object Create<TCreator>(Type instanceType) where TCreator : ClassicCreator
        {
            return Create<TCreator>(instanceType, new object[] { });
        }

        /// <summary>
        /// Создает классы инициализация которых идет через DI
        /// </summary>
        public TInstance Create<TCreator, TInstance>(IServiceProvider provider) where TCreator : SpCreator where TInstance: class
        {
            return Create<TCreator>(provider, typeof(TInstance)) as TInstance;
        }

        /// <summary>
        /// Создает классы инициализация которых идет через DI
        /// </summary>
        public object Create<TCreator>(IServiceProvider provider,Type instanceType) where TCreator : SpCreator 
        {
            Creator creator;

            if (typeof(TCreator) == typeof(ServiceProviderCreator))
            {
                // создаем экземпляр ServiceProviderCreator, через ClassicCreator

                creator = Create<ClassicCreator, ClassicCreator>(new object[] { provider })
                .Create<ServiceProviderCreator>();
            }

            creator = new ServiceProviderPropertyCreator(provider, new object[] { });
            _createdInstances.Add(creator);

            var result = creator.Create(instanceType);
            _createdInstances.Add(result);
            return result;
        }


        /// <summary>
        /// Создает классы, конструктор которых принимает параметры явно, а свойства инициализируеются через DI
        /// </summary>
        public TInstance Create<TCreator, TInstance>(IServiceProvider provider, object[] parameters) where TCreator : ServiceProviderPropertyCreator where TInstance : class
        {
            return Create<TCreator>(provider, typeof(TInstance), parameters) as TInstance;
        }


        /// <summary>
        /// Создает классы, конструктор которых принимает параметры явно, а свойства инициализируеются через DI
        /// </summary>
        public object Create<TCreator>(IServiceProvider provider, Type instanceType, object[] parameters) where TCreator : ServiceProviderPropertyCreator
        {
            var creator = new ServiceProviderPropertyCreator(provider, parameters);
            _createdInstances.Add(creator);

            var result = creator.Create(instanceType);
            _createdInstances.Add(result);
            return result;
        }


        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var createdInstance in _createdInstances)
                    {
                        (createdInstance as IDisposable)?.Dispose();
                    }

                    _createdInstances.Clear();
                }

                _createdInstances = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
