using System;
using System.Collections.Generic;
using System.Linq;

namespace DiLite.Registrations
{
    internal abstract class Registration : IRegistration
    {
        public IReadOnlyList<Type> Aliases { get; }
        public bool IsSingleInstance { get; }

        protected Registration(IEnumerable<Type> aliases, bool isSingleInstance)
        {
            Aliases = aliases.ToList();
            IsSingleInstance = isSingleInstance;
        }

        public abstract object Activate(IContainer container);
    }
}