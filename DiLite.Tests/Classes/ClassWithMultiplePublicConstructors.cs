using DiLite.Tests.Interfaces;

namespace DiLite.Tests.Classes
{
    internal class ClassWithMultiplePublicConstructors
    {
        private readonly IInternalDependency1 _dep1;
        private readonly IInternalDependency2 _dep2;

        public ClassWithMultiplePublicConstructors()
        {
            _dep1 = new InternalDependency1();
            _dep2 = new InternalDependency2();
        }

        public ClassWithMultiplePublicConstructors(IInternalDependency1 dep1, IInternalDependency2 dep2)
        {
            _dep1 = dep1;
            _dep2 = dep2;
        }
    }
}