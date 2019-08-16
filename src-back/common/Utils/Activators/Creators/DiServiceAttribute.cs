using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Activators.Creators
{
    /// <summary>
    /// Указыввается для свойства класса, экземпляр которого берется из контекста DI
    /// Для инициализации класса используется <see cref="ServiceProviderPropertyCreator">
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DiServiceAttribute : Attribute { }
}
