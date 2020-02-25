using DiLite.Exceptions;
using System;
using System.Collections.Generic;

namespace DiLite.Registrations
{
    internal class FactoryMethodRegistration : Registration
    {
        private readonly Func<IContainer, object> _registeredFactoryMethod;

        public FactoryMethodRegistration(Func<IContainer, object> registeredFactoryMethod, IEnumerable<Type> aliases, bool isSingleInstance)
            : base(aliases, isSingleInstance)
        {
            _registeredFactoryMethod = registeredFactoryMethod;
        }

        public override object Activate(IContainer container)
        {
            try
            {
                return _registeredFactoryMethod(container);
            }
            catch (Exception ex)
            {
                throw new InstanceCreationFailedException(ex);
            }
        }
    }
}