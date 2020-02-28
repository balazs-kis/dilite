using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using DiLite.Exceptions;

namespace DiLite.Registrations
{
    internal class RegisteredType : RegisteredEntity
    {
        public RegisteredType(Type type, bool isSingleInstance)
            : base(type, isSingleInstance)
        {
        }

        public override object Activate(IContainer container)
        {
            var publicConstructors = Type.GetConstructors().Where(c => c.IsPublic).ToArray();
            if (publicConstructors.Length != 1)
            {
                throw new ConstructorException(Type);
            }

            return ActivateConstructorWithContainer(publicConstructors.Single(), container);
        }

        private static object ActivateConstructorWithContainer(ConstructorInfo constructorInfo, IContainer container) =>
            constructorInfo.Invoke(
                BindingFlags.CreateInstance,
                null,
                constructorInfo
                    .GetParameters()
                    .Select(p => container.Resolve(p.ParameterType))
                    .ToArray(),
                CultureInfo.CurrentCulture);
    }

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

    internal abstract class RegisteredEntity : IEquatable<RegisteredEntity>
    {
        public Type Type { get; }
        public bool IsSingleInstance { get; }

        protected RegisteredEntity(Type type, bool isSingleInstance)
        {
            Type = type;
            IsSingleInstance = isSingleInstance;
        }

        public override bool Equals(object obj)
        {
            if (obj is RegisteredEntity re)
            {
                return Equals(re);
            }

            return false;
        }

        public bool Equals(RegisteredEntity other)
        {
            if (other != null)
            {
                return Type.FullName == other.Type.FullName;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type.FullName);
        }

        public abstract object Activate(IContainer container);
    }
}