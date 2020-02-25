using System;
using System.Collections.Generic;

namespace DiLite.Registrations
{
    public interface IRegistration
    {
        IReadOnlyList<Type> Aliases { get; }

        bool IsSingleInstance { get; }

        object Activate(IContainer container);
    }
}