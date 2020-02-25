using System;

namespace DiLite.Exceptions
{
    public class ConstructorException : Exception
    {
        public ConstructorException(Type t)
            : base($"Registered types must have exactly one public constructor. '{t.FullName}' has none or more than one public constructors.")
        {
        }
    }
}