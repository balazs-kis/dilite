using DiLite.Tests.Interfaces;

namespace DiLite.Tests.Classes
{
    internal class Main1 : IMain1
    {
        private readonly IDependency1 _dep1;
        private readonly IDependency2 _dep2;

        public Main1(IDependency1 dep1, IDependency2 dep2)
        {
            _dep1 = dep1;
            _dep2 = dep2;
        }
    }
}