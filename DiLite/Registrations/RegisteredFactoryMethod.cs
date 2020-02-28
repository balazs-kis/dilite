using DiLite.Exceptions;
using System;

namespace DiLite.Registrations
{
    internal class RegisteredFactoryMethod : RegisteredEntity
    {
        public Func<IContainer, object> FactoryMethod { get; }

        public RegisteredFactoryMethod(Type type, Func<IContainer, object> factoryMethod, bool isSingleInstance)
            : base(type, isSingleInstance)
        {
            FactoryMethod = factoryMethod;
        }

        public override object Activate(IContainer container)
        {
            try
            {
                return FactoryMethod(container);
            }
            catch (Exception ex)
            {
                throw new InstanceCreationFailedException(ex);
            }
        }
    }
}