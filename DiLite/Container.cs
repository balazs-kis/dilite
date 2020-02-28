using DiLite.Exceptions;
using DiLite.Registrations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiLite
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, List<RegisteredEntity>> _registrationsByType;
        private readonly Dictionary<RegisteredEntity, object> _singleInstances;


        internal Container(IEnumerable<Registration> registrations)
        {
            _singleInstances = new Dictionary<RegisteredEntity, object>();
            _registrationsByType = new Dictionary<Type, List<RegisteredEntity>>();

            foreach (var registration in registrations)
            {
                var registeredEntity = registration.RegisteredEntity;
                foreach (var alias in registration.Aliases)
                {
                    if (_registrationsByType.ContainsKey(alias) &&
                        !_registrationsByType[alias].Contains(registeredEntity))
                    {
                        _registrationsByType[alias].Add(registeredEntity);
                    }
                    else
                    {
                        _registrationsByType[alias] = new List<RegisteredEntity> { registeredEntity };
                    }
                }
            }
        }


        public T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type t)
        {
            CheckIfRegistered(t);

            var registration = _registrationsByType[t].Last();

            try
            {
                return GetOrCreateInstance(registration);
            }
            catch (NotRegisteredException nre)
            {
                throw new DependencyResolutionException(t, nre);
            }
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return ResolveAll(typeof(T)).Cast<T>().ToArray();
        }

        public IEnumerable<object> ResolveAll(Type t)
        {
            CheckIfRegistered(t);

            var registrations = _registrationsByType[t];

            try
            {
                return registrations.Select(GetOrCreateInstance);
            }
            catch (NotRegisteredException nre)
            {
                throw new DependencyResolutionException(t, nre);
            }
        }


        private object GetOrCreateInstance(RegisteredEntity registeredEntity)
        {
            if (!registeredEntity.IsSingleInstance)
            {
                return registeredEntity.Activate(this);
            }

            if (_singleInstances.ContainsKey(registeredEntity))
            {
                return _singleInstances[registeredEntity];
            }

            var result = registeredEntity.Activate(this);
            _singleInstances[registeredEntity] = result;

            return result;
        }

        private void CheckIfRegistered(Type t)
        {
            if (!_registrationsByType.ContainsKey(t))
            {
                throw new NotRegisteredException(t);
            }
        }
    }
}