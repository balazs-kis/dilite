using System;
using System.Collections.Generic;

namespace DiLite.Registrations
{
    internal class TypeRegistration : Registration
    {
        public TypeRegistration(
            Type registeredType,
            IEnumerable<Type> aliases,
            bool isSingleInstance)
            : base(
                registeredType,
                aliases,
                isSingleInstance)
        {
        }

        public override RegisteredEntity RegisteredEntity => new RegisteredType(RegisteredType, IsSingleInstance);
    }
}