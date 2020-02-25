using System;

namespace DiLite.Exceptions
{
    public class InstanceCreationFailedException : Exception
    {
        public InstanceCreationFailedException(Exception innerException)
            : base(
                "Instance creation with factory method failed. The factory method threw an exception (see the InnerException for details).",
                innerException)
        {
        }
    }
}