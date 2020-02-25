using System;

namespace DiLite.Exceptions
{
    public class NotInstantiableTypeException : Exception
    {
        public NotInstantiableTypeException(Type t)
            : base($"The type '{t.FullName}' cannot be instantiated.")
        {
        }
    }
}