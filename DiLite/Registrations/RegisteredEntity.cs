using System;

namespace DiLite.Registrations
{
    internal abstract class RegisteredEntity : IEquatable<RegisteredEntity>
    {
        public Type Type { get; }
        public bool IsSingleInstance { get; }

        protected RegisteredEntity(Type type, bool isSingleInstance)
        {
            Type = type;
            IsSingleInstance = isSingleInstance;
        }

        public override bool Equals(object obj)
        {
            if (obj is RegisteredEntity re)
            {
                return Equals(re);
            }

            return false;
        }

        public bool Equals(RegisteredEntity other)
        {
            if (other != null)
            {
                return Type.FullName == other.Type.FullName;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type.FullName);
        }

        public abstract object Activate(IContainer container);
    }
}