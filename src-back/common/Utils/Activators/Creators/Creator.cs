using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Activators.Creators
{
    public abstract class Creator
    {
        public T Create<T>() where T : class
        {
            return Create(typeof(T)) as T;
        }

        public abstract object Create(Type typeOfInstance);
    }
}
