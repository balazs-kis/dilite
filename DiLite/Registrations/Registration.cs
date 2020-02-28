using System;
using System.Collections.Generic;
using System.Linq;

namespace DiLite.Registrations
{
    internal abstract class Registration
    {
        protected readonly Type RegisteredType;
        protected readonly bool IsSingleInstance;

        public IReadOnlyList<Type> Aliases { get; }
        public abstract RegisteredEntity RegisteredEntity { get; }

        protected Registration(Type registeredType, IEnumerable<Type> aliases, bool isSingleInstance)
        {
            RegisteredType = registeredType;
            Aliases = aliases.ToList();
            IsSingleInstance = isSingleInstance;
        }
    }
}