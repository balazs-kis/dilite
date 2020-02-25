using System;

namespace DiLite.Exceptions
{
    public class DependencyResolutionException : Exception
    {
        public DependencyResolutionException(Type t, NotRegisteredException innerException)
            : base(
                $"The type '{t.FullName}' cannot be resolved because one or more of its dependencies was not registered. See inner exception for details.",
                innerException)
        {
        }
    }
}