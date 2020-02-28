using DiLite.Exceptions;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

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
}