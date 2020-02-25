using DiLite.Builders;
using DiLite.Tests.Classes;
using DiLite.Tests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DiLite.Tests
{
    [TestClass]
    public class TestResolutions
    {
        [TestMethod]
        public void TestResolutionWithInterface()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Main1>().As<IMain1>();
            RegisterAllDependenciesAsInterface(containerBuilder);
            var container = containerBuilder.Build();

            var main = container.Resolve<IMain1>();

            Assert.IsNotNull(main);
        }

        [TestMethod]
        public void TestResolutionAsSelf()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Main2>();
            RegisterAllDependenciesAsSelf(containerBuilder);
            var container = containerBuilder.Build();

            var main2 = container.Resolve<Main2>();

            Assert.IsNotNull(main2);
        }

        [TestMethod]
        public void TestResolutionWithInterfaceAndAsSelf()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Main1>().As<IMain1>();
            containerBuilder.RegisterType<Main2>();
            RegisterAllDependenciesAsInterfaceAndSelf(containerBuilder);
            var container = containerBuilder.Build();

            var main1 = container.Resolve<IMain1>();
            var main2 = container.Resolve<Main2>();

            Assert.IsNotNull(main1);
            Assert.IsNotNull(main2);
        }

        [TestMethod]
        public void TestResolutionWithFactoryMethodAsSelf()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterAllDependenciesAsInterface(containerBuilder);

            containerBuilder.RegisterFactoryMethod(context =>
            {
                var dep1 = context.Resolve<IDependency1>();
                var dep2 = context.Resolve<IDependency2>();

                return new Main1(dep1, dep2);
            });

            var container = containerBuilder.Build();

            var main = container.Resolve<Main1>();

            Assert.IsNotNull(main);
        }

        [TestMethod]
        public void TestResolutionWithFactoryMethodWithInterface()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterAllDependenciesAsInterface(containerBuilder);

            containerBuilder.RegisterFactoryMethod(context =>
            {
                var dep1 = context.Resolve<IDependency1>();
                var dep2 = context.Resolve<IDependency2>();

                return new Main1(dep1, dep2);
            }).As<IMain1>();

            var container = containerBuilder.Build();

            var main = container.Resolve<IMain1>();

            Assert.IsNotNull(main);
        }

        [TestMethod]
        public void TestResolutionWithFactoryMethodWithInterfaceAndAsSelf()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterAllDependenciesAsInterface(containerBuilder);

            containerBuilder.RegisterFactoryMethod(context =>
            {
                var dep1 = context.Resolve<IDependency1>();
                var dep2 = context.Resolve<IDependency2>();

                return new Main1(dep1, dep2);
            }).As<IMain1>().AsSelf();

            var container = containerBuilder.Build();

            var main1 = container.Resolve<IMain1>();
            var main2 = container.Resolve<Main1>();

            Assert.IsNotNull(main1);
            Assert.IsNotNull(main2);
        }

        [TestMethod]
        public void TestSingleRegistrationEnumerableResolution()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Common1>().As<ICommonInterface>();
            var container = containerBuilder.Build();

            var resolved = container.ResolveAll<ICommonInterface>().ToList();

            Assert.AreEqual(1, resolved.Count);
            Assert.IsInstanceOfType(resolved.Single(), typeof(Common1));
        }

        [TestMethod]
        public void TestMultipleRegistrationEnumerableResolution()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Common1>().As<ICommonInterface>();
            containerBuilder.RegisterType<Common2>().As<ICommonInterface>();
            var container = containerBuilder.Build();

            var resolved = container.ResolveAll<ICommonInterface>().ToList();

            Assert.AreEqual(2, resolved.Count);
            Assert.IsNotNull(resolved.OfType<Common1>().SingleOrDefault());
            Assert.IsNotNull(resolved.OfType<Common2>().SingleOrDefault());
        }

        [TestMethod]
        public void TestMultipleRegistrationsLastResolved()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Common1>().As<ICommonInterface>();
            containerBuilder.RegisterType<Common2>().As<ICommonInterface>();
            var container = containerBuilder.Build();

            var resolved = container.Resolve<ICommonInterface>();

            Assert.IsNotNull(resolved);
            Assert.IsInstanceOfType(resolved, typeof(Common2));
        }


        private void RegisterAllDependenciesAsInterface(IContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<Dependency1>().As<IDependency1>();
            containerBuilder.RegisterType<Dependency2>().As<IDependency2>();
            containerBuilder.RegisterType<Dependency3>().As<IDependency3>();
            containerBuilder.RegisterType<Dependency4>().As<IDependency4>();

            containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>();
            containerBuilder.RegisterType<InternalDependency2>().As<IInternalDependency2>();
            containerBuilder.RegisterType<InternalDependency3>().As<IInternalDependency3>();
            containerBuilder.RegisterType<InternalDependency4>().As<IInternalDependency4>();
        }

        private void RegisterAllDependenciesAsSelf(IContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<Dependency1>();
            containerBuilder.RegisterType<Dependency2>();
            containerBuilder.RegisterType<Dependency3>();
            containerBuilder.RegisterType<Dependency4>();

            containerBuilder.RegisterType<InternalDependency1>();
            containerBuilder.RegisterType<InternalDependency2>();
            containerBuilder.RegisterType<InternalDependency3>();
            containerBuilder.RegisterType<InternalDependency4>();
        }

        private void RegisterAllDependenciesAsInterfaceAndSelf(IContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<Dependency1>().As<IDependency1>().AsSelf();
            containerBuilder.RegisterType<Dependency2>().As<IDependency2>().AsSelf();
            containerBuilder.RegisterType<Dependency3>().As<IDependency3>().AsSelf();
            containerBuilder.RegisterType<Dependency4>().As<IDependency4>().AsSelf();

            containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>().AsSelf();
            containerBuilder.RegisterType<InternalDependency2>().As<IInternalDependency2>().AsSelf();
            containerBuilder.RegisterType<InternalDependency3>().As<IInternalDependency3>().AsSelf();
            containerBuilder.RegisterType<InternalDependency4>().As<IInternalDependency4>().AsSelf();
        }
    }
}