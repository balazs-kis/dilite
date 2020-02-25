using DiLite.Tests.Interfaces;

namespace DiLite.Tests.Classes
{
    internal class Main2 : IMain2
    {
        private readonly Dependency3 _dep3;
        private readonly Dependency4 _dep4;

        public Main2(Dependency3 dep3, Dependency4 dep4)
        {
            _dep3 = dep3;
            _dep4 = dep4;
        }
    }
}