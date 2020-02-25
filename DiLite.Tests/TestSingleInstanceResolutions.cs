using DiLite.Builders;
using DiLite.Tests.Classes;
using DiLite.Tests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiLite.Tests
{
    [TestClass]
    public class TestSingleInstanceResolutions
    {
        [TestMethod]
        public void TestNonSingleInstanceCreatedEachTime()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>();
            var container = containerBuilder.Build();

            var resolvedFirst = container.Resolve<IInternalDependency1>();
            resolvedFirst.IntProperty = 42;
            var resolvedSecond = container.Resolve<IInternalDependency1>();

            Assert.AreNotSame(resolvedFirst, resolvedSecond);
            Assert.AreNotEqual(42, resolvedSecond.IntProperty);
        }

        [TestMethod]
        public void TestSingleInstanceCreatedOnce()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>().AsSingleInstance();
            var container = containerBuilder.Build();

            var resolvedFirst = container.Resolve<IInternalDependency1>();
            resolvedFirst.IntProperty = 42;
            var resolvedSecond = container.Resolve<IInternalDependency1>();

            Assert.AreSame(resolvedFirst, resolvedSecond);
            Assert.AreEqual(42, resolvedSecond.IntProperty);
        }
    }
}