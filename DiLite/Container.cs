using DiLite.Exceptions;
using DiLite.Registrations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiLite
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, List<IRegistration>> _registrationsByType;
        private readonly Dictionary<IRegistration, object> _singleInstances;


        public Container(IEnumerable<IRegistration> registrations)
        {
            _singleInstances = new Dictionary<IRegistration, object>();
            _registrationsByType = new Dictionary<Type, List<IRegistration>>();

            foreach (var registration in registrations)
            {
                foreach (var alias in registration.Aliases)
                {
                    if (_registrationsByType.ContainsKey(alias))
                    {
                        _registrationsByType[alias].Add(registration);
                    }
                    else
                    {
                        _registrationsByType[alias] = new List<IRegistration> { registration };
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


        private object GetOrCreateInstance(IRegistration registration)
        {
            if (!registration.IsSingleInstance)
            {
                return registration.Activate(this);
            }

            if (_singleInstances.ContainsKey(registration))
            {
                return _singleInstances[registration];
            }

            var result = registration.Activate(this);
            _singleInstances[registration] = result;

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