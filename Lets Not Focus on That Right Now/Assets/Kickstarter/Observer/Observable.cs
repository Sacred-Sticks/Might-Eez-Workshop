using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kickstarter.Observer
{
    public abstract class Observable : MonoBehaviour
    {
        private readonly Dictionary<Type, List<object>> observerLists = new Dictionary<Type, List<object>>();

        public void AddObserver(object observer)
        {
            var observerType = observer.GetType();
            var interfaceTypes = observerType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IObserver<>));

            foreach (var interfaceType in interfaceTypes)
            {
                var genericType = interfaceType.GetGenericArguments()[0];

                if (!observerLists.ContainsKey(genericType))
                    observerLists.Add(genericType, new List<object>());

                var observers = GetObserverList(genericType);

                observers.Add(observer);
            }
        }
        
        private IList GetObserverList(Type type)
        {
            if (observerLists.ContainsKey(type))
                return observerLists[type];
            throw new KeyNotFoundException($"No observers found for type {type.Name}");
        }

        public void RemoveObserver(object observer)
        {
            var observerType = observer.GetType();
            var interfaceTypes = observerType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IObserver<>));

            foreach (var interfaceType in interfaceTypes)
            {
                var genericType = interfaceType.GetGenericArguments()[0];

                if (!observerLists.ContainsKey(genericType))
                    observerLists.Add(genericType, new List<object>());

                var observers = GetObserverList(genericType);

                observers.Remove(observer);
            }
        }
        
        protected void NotifyObservers<T>(T argument)
        {
            if (!observerLists.ContainsKey(typeof(T)))
                return;
            var observers = GetObserverList<T>();
            for (int i = observers.Count - 1; i >= 0; i--)
                if (observers[i] is IObserver<T> observer)
                    observer.OnNotify(argument);
        }

        private List<object> GetObserverList<T>()
        {
            return observerLists[typeof(T)];
        }
    }
}
