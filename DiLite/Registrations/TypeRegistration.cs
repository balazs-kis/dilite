using DiLite.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace DiLite.Registrations
{
    internal class TypeRegistration : Registration
    {
        private readonly Type _registeredType;

        public TypeRegistration(Type registeredType, IEnumerable<Type> aliases, bool isSingleInstance)
            : base(aliases, isSingleInstance)
        {
            _registeredType = registeredType;
        }

        public override object Activate(IContainer container)
        {
            var publicConstructors = _registeredType.GetConstructors().Where(c => c.IsPublic).ToArray();
            if (publicConstructors.Length != 1)
            {
                throw new ConstructorException(_registeredType);
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