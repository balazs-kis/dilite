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
        public void RegisterTypeWithoutAlias_ResolvedSuccessfully()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Main2>();
            RegisterAllDependenciesAsSelf(containerBuilder);
            var container = containerBuilder.Build();

            var main2 = container.Resolve<Main2>();

            Assert.IsNotNull(main2,
                "Resolved instance should not be null");
        }

        [TestMethod]
        public void RegisterTypeAsInterface_ResolvedSuccessfully()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Main1>().As<IMain1>();
            RegisterAllDependenciesAsInterface(containerBuilder);
            var container = containerBuilder.Build();

            var main = container.Resolve<IMain1>();

            Assert.IsNotNull(main,
                "Resolved instance should not be null");
        }

        [TestMethod]
        public void RegisterTypeAsInterfaceAndAsSelf_BothResolvedSuccessfully()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Main1>().As<IMain1>();
            containerBuilder.RegisterType<Main2>();
            RegisterAllDependenciesAsInterfaceAndSelf(containerBuilder);
            var container = containerBuilder.Build();

            var main1 = container.Resolve<IMain1>();
            var main2 = container.Resolve<Main2>();

            Assert.IsNotNull(main1,
                "Resolved instance should not be null");

            Assert.IsNotNull(main2,
                "Resolved instance should not be null");
        }

        [TestMethod]
        public void RegisterFactoryMethodWithoutAlias_ResolvedSuccessfully()
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

            Assert.IsNotNull(main,
                "Resolved instance should not be null");
        }

        [TestMethod]
        public void RegisterFactoryMethodAsInterface_ResolvedSuccessfully()
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

            Assert.IsNotNull(main,
                "Resolved instance should not be null");
        }

        [TestMethod]
        public void RegisterFactoryMethodAsInterfaceAndAsSelf_BothResolvedSuccessfully()
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

            Assert.IsNotNull(main1,
                "Resolved instance should not be null");

            Assert.IsNotNull(main2,
                "Resolved instance should not be null");
        }

        [TestMethod]
        public void RegisterOneClassForAlias_ResolveAllReturnsSuccessfullyWithOneItem()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Common1>().As<ICommonInterface>();
            var container = containerBuilder.Build();

            var resolved = container.ResolveAll<ICommonInterface>().ToList();

            Assert.AreEqual(1, resolved.Count,
                "The Resolution should return only one instance");

            Assert.IsInstanceOfType(resolved.Single(), typeof(Common1),
                "The resolved type should match the registered type");
        }

        [TestMethod]
        public void RegisterMultipleClassesForAlias_LastResolvedSuccessfully()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Common1>().As<ICommonInterface>();
            containerBuilder.RegisterType<Common2>().As<ICommonInterface>();
            var container = containerBuilder.Build();

            var resolved = container.Resolve<ICommonInterface>();

            Assert.IsNotNull(resolved,
                "Resolved instance should not be null");

            Assert.IsInstanceOfType(resolved, typeof(Common2),
                "The resolved type should match the last registered type");
        }

        [TestMethod]
        public void RegisterMultipleClassesForAlias_ResolveAllReturnsSuccessfullyWithOneItem()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Common1>().As<ICommonInterface>();
            containerBuilder.RegisterType<Common2>().As<ICommonInterface>();
            var container = containerBuilder.Build();

            var resolved = container.ResolveAll<ICommonInterface>().ToList();

            Assert.AreEqual(2, resolved.Count,
                "The Resolution should return 2 instances");

            Assert.IsNotNull(resolved.OfType<Common1>().SingleOrDefault(),
                $"One resolved instance should be of type '{typeof(Common1).Name}'");

            Assert.IsNotNull(resolved.OfType<Common2>().SingleOrDefault(),
                $"One resolved instance should be of type '{typeof(Common2).Name}'");
        }
        
        [TestMethod]
        public void RegisterTypeAsNonSingleInstance_NewInstanceCreatedForEachResolve()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>();
            var container = containerBuilder.Build();

            var resolvedFirst = container.Resolve<IInternalDependency1>();
            resolvedFirst.IntProperty = 42;
            var resolvedSecond = container.Resolve<IInternalDependency1>();

            Assert.AreNotSame(resolvedFirst, resolvedSecond,
                "The two resolved instances should be different");

            Assert.AreNotEqual(42, resolvedSecond.IntProperty,
                "The value should not be set on the second instance");
        }

        [TestMethod]
        public void RegisterFactoryMethodAsNonSingleInstance_NewInstanceCreatedForEachResolve()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterFactoryMethod(c => new InternalDependency1()).As<IInternalDependency1>();
            var container = containerBuilder.Build();

            var resolvedFirst = container.Resolve<IInternalDependency1>();
            resolvedFirst.IntProperty = 42;
            var resolvedSecond = container.Resolve<IInternalDependency1>();

            Assert.AreNotSame(resolvedFirst, resolvedSecond,
                "The two resolved instances should be different");

            Assert.AreNotEqual(42, resolvedSecond.IntProperty,
                "The value should not be set on the second instance");
        }

        [TestMethod]
        public void RegisterTypeAsSingleInstance_SameInstanceReturnedForEachResolve()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>().AsSingleInstance();
            var container = containerBuilder.Build();

            var resolvedFirst = container.Resolve<IInternalDependency1>();
            resolvedFirst.IntProperty = 42;
            var resolvedSecond = container.Resolve<IInternalDependency1>();

            Assert.AreSame(resolvedFirst, resolvedSecond,
                "The two resolved instances should be the same");

            Assert.AreEqual(42, resolvedSecond.IntProperty,
                "The value should be set on the single instance");
        }

        [TestMethod]
        public void RegisterFactoryMethodAsSingleInstance_SameInstanceReturnedForEachResolve()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterFactoryMethod(c => new InternalDependency1()).As<IInternalDependency1>().AsSingleInstance();
            var container = containerBuilder.Build();

            var resolvedFirst = container.Resolve<IInternalDependency1>();
            resolvedFirst.IntProperty = 42;
            var resolvedSecond = container.Resolve<IInternalDependency1>();

            Assert.AreSame(resolvedFirst, resolvedSecond,
                "The two resolved instances should be the same");

            Assert.AreEqual(42, resolvedSecond.IntProperty,
                "The value should be set on the single instance");
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