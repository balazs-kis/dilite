using DiLite.Exceptions;
using DiLite.Registrations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiLite.Builders
{
    internal class RegistrationBuilder : BuilderBase<Registration>, IRegistrationBuilder
    {
        private enum RegistrationCategory { Type, FactoryMethod }

        private readonly List<Type> _registeredAliases;

        private readonly RegistrationCategory _category;
        private readonly Type _registeredType;
        private readonly Func<IContainer, object> _registeredFactoryMethod;
        private bool _isSingleInstance;


        public RegistrationBuilder(Type t) : this()
        {
            CheckTypeValidity(t);

            _category = RegistrationCategory.Type;
            _registeredType = t;
        }

        public RegistrationBuilder(Func<IContainer, object> factoryMethod, Type t) : this(t)
        {
            _category = RegistrationCategory.FactoryMethod;
            _registeredFactoryMethod = factoryMethod;
        }

        public RegistrationBuilder()
        {
            _registeredAliases = new List<Type>();
            _isSingleInstance = false;
        }


        public IRegistrationBuilder As<T>()
        {
            return As(typeof(T));
        }

        public IRegistrationBuilder As(Type t)
        {
            // ReSharper disable once CheckForReferenceEqualityInstead.1
            if (t.Equals(_registeredType))
            {
                throw new InvalidOperationException("Cannot register type as itself this way. Use the 'AsSelf()' method instead.");
            }

            if (!t.IsAssignableFrom(_registeredType))
            {
                throw new InvalidOperationException($"The type '{_registeredType.Name}' cannot be assigned to '{t.Name}'.");
            }

            _registeredAliases.Add(t);
            return this;
        }

        public IRegistrationBuilder AsSelf()
        {
            _registeredAliases.Add(_registeredType);
            return this;
        }

        public IRegistrationBuilder AsSingleInstance()
        {
            _isSingleInstance = true;
            return this;
        }

        protected override Registration BuildInternal()
        {
            if (!_registeredAliases.Any())
            {
                AsSelf();
            }

            switch (_category)
            {
                case RegistrationCategory.Type:
                    return new TypeRegistration(
                        _registeredType,
                        _registeredAliases,
                        _isSingleInstance);

                case RegistrationCategory.FactoryMethod:
                    return new FactoryMethodRegistration(
                        _registeredFactoryMethod,
                        _registeredType,
                        _registeredAliases,
                        _isSingleInstance);

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(_category), $"Unknown registration category {_category}");
            }
        }


        private static void CheckTypeValidity(Type t)
        {
            if (!t.IsClass || t.IsAbstract)
            {
                throw new NotInstantiableTypeException(t);
            }
        }
    }
}