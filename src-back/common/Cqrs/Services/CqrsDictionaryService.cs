using Cqrs.Commands.Command;
using Cqrs.Interfaces;
using Cqrs.Queries.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs.Services
{
    public class CqrsDictionaryService: Dictionary<Type,Type>, ICqrsDictionaryService
    {
        private Type[] AssemblyTypes { get; set; }

        public CqrsDictionaryService(Type[] TypesFromAsseblies)
        {
            AssemblyTypes = TypesFromAsseblies
                .Select(t=>t.Assembly)
                .SelectMany(a=> a.GetTypes())
                .ToArray();

            Initialize();
        }

        public Type GetHandlerType(Type actionType)
        {
            if (!base.TryGetValue(actionType, out var value))
            {
                throw new ApplicationException($"Архитектурная ошибка: Класс {actionType.Name}. Не содержится в словаре {nameof(CqrsDictionaryService)}. Скорее всего вы использовали функцию получения обработчика вне сервиса {nameof(ICqrsService)}");
            }
            return value;
        }

        /// <summary>
        /// Добавление соответствий типов в словарь
        /// class:ICommand - class:ICommandHandler
        /// class:IQuery - class:IQueryHandler
        /// </summary>
        private void Initialize()
        {
            Add<ICommand, ICommandHandler<Command>>();
            Add<IQuery<object>, IQueryHandler<Query,object>>();
        }

        /// <summary>
        /// Собирает словарь соотвтетствия class:IAction - class:IActionHandler<Action>
        /// </summary>
        /// <typeparam name="TAction"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        private void Add<TAction, THandler>() 
        {
            // все типы в текущем проекте реализующие интерфейс TAction
            var actionTypes = AssemblyTypes.Where(type => !type.IsAbstract && IsImplementGenericInterface<TAction>(type))
                .Where(type => typeof(TAction) != type)
                .ToList();

            // основное название generic интерфейса THandler
            var iHandlerName = GetName<THandler>();

            // все классы релизующие generic интерфейс THandler
            var handlerTypes = AssemblyTypes.Where(type => !type.IsInterface && !type.IsAbstract && IsImplementGenericInterface<THandler>(type))
                .ToList();

            foreach (var actionType in actionTypes)
            {
                // все классы реализующие интерфейс THandler<actionType>
                var actionHandlers = handlerTypes.Where(handler => handler.GetInterfaces().Any(i => i.GenericTypeArguments.Any(a => a == actionType)))
                    .ToList();

                // должна быть только одна реализация интерфейса THandler<actionType>
                if (actionHandlers.Count > 1)
                {
                    throw new ApplicationException($"Архитектурная ошибка: Класс {actionType.FullName} должен иметь только один обработчик {iHandlerName}<{actionType.Name}>");
                }
                else if (actionHandlers.Count == 0)
                {
                    throw new ApplicationException($"Архитектурная ошибка: Класс {actionType.FullName} Не имеет ни одного обработчика {iHandlerName}<{actionType.Name}>");
                }

                var actionHandler = actionHandlers.First();

                base.Add(actionType, actionHandler);
            }
        }

        /// <summary>
        /// Явлется ли класс реализацией интерфейса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsImplementGenericInterface<T>(Type type)
        {
            var typeT = typeof(T);
            return type.GetInterfaces().Any(i => i.Name == typeT.Name);
        }

        /// <summary>
        /// Основное имя generic-интерфейса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private string GetName<T>()
        {
            var typeName = typeof(T).Name;
            var p = typeName.IndexOf('`');
            return typeName.Substring(0, p + 1);
        }
    }
}
