using DiLite.Tests.Interfaces;

namespace DiLite.Tests.Classes
{
    internal class Dependency4 : IDependency4
    {
        private readonly InternalDependency2 _internalDep2;
        private readonly InternalDependency3 _internalDep3;
        private readonly InternalDependency4 _internalDep4;

        public Dependency4(InternalDependency2 internalDep2, InternalDependency3 internalDep3, InternalDependency4 internalDep4)
        {
            _internalDep2 = internalDep2;
            _internalDep3 = internalDep3;
            _internalDep4 = internalDep4;
        }
    }
}