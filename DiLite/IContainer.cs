using System;
using System.Collections.Generic;

namespace DiLite
{
    public interface IContainer
    {
        T Resolve<T>() where T : class;
        object Resolve(Type t);

        IEnumerable<T> ResolveAll<T>() where T : class;
        IEnumerable<object> ResolveAll(Type t);
    }
}