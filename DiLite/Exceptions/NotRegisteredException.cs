using System;

namespace DiLite.Exceptions
{
    public class NotRegisteredException : Exception
    {
        public NotRegisteredException(Type t)
            : base($"The type '{t.FullName}' was not registered.")
        {
        }
    }
}