using System;
using System.Collections.Generic;

namespace DiLite.Registrations
{
    internal class FactoryMethodRegistration : Registration
    {
        private readonly Func<IContainer, object> _registeredFactoryMethod;

        public FactoryMethodRegistration(
            Func<IContainer, object> registeredFactoryMethod,
            Type createdType,
            IEnumerable<Type> aliases,
            bool isSingleInstance)
            : base(
                createdType,
                aliases,
                isSingleInstance)
        {
            _registeredFactoryMethod = registeredFactoryMethod;
        }

        public override RegisteredEntity RegisteredEntity => new RegisteredFactoryMethod(RegisteredType, _registeredFactoryMethod, IsSingleInstance);
    }
}