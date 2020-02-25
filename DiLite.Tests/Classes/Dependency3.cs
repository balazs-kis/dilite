using DiLite.Tests.Interfaces;

namespace DiLite.Tests.Classes
{
    internal class Dependency3 : IDependency3
    {
        private readonly InternalDependency1 _internalDep1;
        private readonly InternalDependency2 _internalDep2;
        private readonly InternalDependency3 _internalDep3;

        public Dependency3(InternalDependency1 internalDep1, InternalDependency2 internalDep2, InternalDependency3 internalDep3)
        {
            _internalDep1 = internalDep1;
            _internalDep2 = internalDep2;
            _internalDep3 = internalDep3;
        }
    }
}