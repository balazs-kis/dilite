using DiLite.Tests.Interfaces;

namespace DiLite.Tests.Classes
{
    internal class Dependency2 : IDependency2
    {
        private readonly IInternalDependency2 _internalDep2;
        private readonly IInternalDependency3 _internalDep3;
        private readonly IInternalDependency4 _internalDep4;

        public Dependency2(IInternalDependency2 internalDep2, IInternalDependency3 internalDep3, IInternalDependency4 internalDep4)
        {
            _internalDep2 = internalDep2;
            _internalDep3 = internalDep3;
            _internalDep4 = internalDep4;
        }
    }
}