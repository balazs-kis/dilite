using DiLite.Tests.Interfaces;

namespace DiLite.Tests.Classes
{
    internal class Dependency1 : IDependency1
    {
        private readonly IInternalDependency1 _internalDep1;
        private readonly IInternalDependency2 _internalDep2;
        private readonly IInternalDependency3 _internalDep3;

        public Dependency1(IInternalDependency1 internalDep1, IInternalDependency2 internalDep2, IInternalDependency3 internalDep3)
        {
            _internalDep1 = internalDep1;
            _internalDep2 = internalDep2;
            _internalDep3 = internalDep3;
        }
    }
}