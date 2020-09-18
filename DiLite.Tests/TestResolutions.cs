using DiLite.Builders;
using DiLite.Tests.Classes;
using DiLite.Tests.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TestLite;

namespace DiLite.Tests
{
    [TestClass]
    public class TestResolutions
    {
        [TestMethod]
        public void RegisterTypeWithoutAlias_ResolvedSuccessfully() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<Main2>();
                RegisterAllDependenciesAsSelf(containerBuilder);
                return containerBuilder.Build();
            })
            .Act(container => container.Resolve<Main2>())
            .Assert()
                .Validate(result => result.Should().NotBeNull("Resolved instance should not be null"));


        [TestMethod]
        public void RegisterTypeAsInterface_ResolvedSuccessfully() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<Main1>().As<IMain1>();
                RegisterAllDependenciesAsInterface(containerBuilder);
                return containerBuilder.Build();
            })
            .Act(container => container.Resolve<IMain1>())
            .Assert()
                .Validate(result => result.Should().NotBeNull("Resolved instance should not be null"));

        [TestMethod]
        public void RegisterTypeAsInterfaceAndAsSelf_BothResolvedSuccessfully() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<Main1>().As<IMain1>();
                containerBuilder.RegisterType<Main2>();
                RegisterAllDependenciesAsInterfaceAndSelf(containerBuilder);
                return containerBuilder.Build();
            })
            .Act(container =>
            {
                var main1 = container.Resolve<IMain1>();
                var main2 = container.Resolve<Main2>();
                return (main1, main2);
            })
            .Assert()
                .Validate(result => result.main1.Should().NotBeNull("Resolved instance should not be null"))
                .Validate(result => result.main2.Should().NotBeNull("Resolved instance should not be null"));

        [TestMethod]
        public void RegisterFactoryMethodWithoutAlias_ResolvedSuccessfully() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                RegisterAllDependenciesAsInterface(containerBuilder);

                containerBuilder.RegisterFactoryMethod(context =>
                {
                    var dep1 = context.Resolve<IDependency1>();
                    var dep2 = context.Resolve<IDependency2>();

                    return new Main1(dep1, dep2);
                });

                return containerBuilder.Build();
            })
            .Act(container => container.Resolve<Main1>())
            .Assert()
                .Validate(result => result.Should().NotBeNull("Resolved instance should not be null"));

        [TestMethod]
        public void RegisterFactoryMethodAsInterface_ResolvedSuccessfully() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                RegisterAllDependenciesAsInterface(containerBuilder);

                containerBuilder.RegisterFactoryMethod(context =>
                {
                    var dep1 = context.Resolve<IDependency1>();
                    var dep2 = context.Resolve<IDependency2>();

                    return new Main1(dep1, dep2);
                }).As<IMain1>();

                return containerBuilder.Build();
            })
            .Act(container => container.Resolve<IMain1>())
            .Assert()
                .Validate(result => result.Should().NotBeNull("Resolved instance should not be null"));

        [TestMethod]
        public void RegisterFactoryMethodAsInterfaceAndAsSelf_BothResolvedSuccessfully() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                RegisterAllDependenciesAsInterface(containerBuilder);

                containerBuilder.RegisterFactoryMethod(context =>
                {
                    var dep1 = context.Resolve<IDependency1>();
                    var dep2 = context.Resolve<IDependency2>();

                    return new Main1(dep1, dep2);
                }).As<IMain1>().AsSelf();

                return containerBuilder.Build();
            })
            .Act(container =>
            {
                var main1 = container.Resolve<IMain1>();
                var main2 = container.Resolve<Main1>();
                return (main1, main2);
            })
            .Assert()
                .Validate(result => result.main1.Should().NotBeNull("Resolved instance should not be null"))
                .Validate(result => result.main2.Should().NotBeNull("Resolved instance should not be null"));

        [TestMethod]
        public void RegisterOneClassForAlias_ResolveAllReturnsSuccessfullyWithOneItem() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<Common1>().As<ICommonInterface>();
                return containerBuilder.Build();
            })
            .Act(container => container.ResolveAll<ICommonInterface>().ToList())
            .Assert()
                .Validate(result => result.Count.Should().Be(1, "The Resolution should return only one instance"))
                .Validate(result => result.Single().Should().BeOfType<Common1>("The resolved type should match the registered type"));

        [TestMethod]
        public void RegisterMultipleClassesForAlias_LastResolvedSuccessfully() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<Common1>().As<ICommonInterface>();
                containerBuilder.RegisterType<Common2>().As<ICommonInterface>();
                return containerBuilder.Build();
            })
            .Act(container => container.Resolve<ICommonInterface>())
            .Assert()
                .Validate(result => result.Should().NotBeNull("Resolved instance should not be null"))
                .Validate(result => result.Should().BeOfType<Common2>("The resolved type should match the last registered type"));

        [TestMethod]
        public void RegisterMultipleClassesForAlias_ResolveAllReturnsSuccessfullyWithMultipleItems() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<Common1>().As<ICommonInterface>();
                containerBuilder.RegisterType<Common2>().As<ICommonInterface>();
                return containerBuilder.Build();
            })
            .Act(container => container.ResolveAll<ICommonInterface>().ToList())
            .Assert()
                .Validate(result => result.Count.Should().Be(2, "The Resolution should return 2 instances"))
                .Validate(result => result.OfType<Common1>().FirstOrDefault().Should().NotBeNull($"One resolved instance should be of type '{typeof(Common1).Name}'"))
                .Validate(result => result.OfType<Common2>().FirstOrDefault().Should().NotBeNull($"One resolved instance should be of type '{typeof(Common2).Name}'"));

        [TestMethod]
        public void RegisterTypeAsNonSingleInstance_NewInstanceCreatedForEachResolve() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>();
                return containerBuilder.Build();
            })
            .Act(container =>
            {
                var resolvedFirst = container.Resolve<IInternalDependency1>();
                resolvedFirst.IntProperty = 42;
                var resolvedSecond = container.Resolve<IInternalDependency1>();

                return (resolvedFirst, resolvedSecond);
            })
            .Assert()
                .Validate(result => result.resolvedFirst.Should().NotBeSameAs(result.resolvedSecond, "The two resolved instances should be different"))
                .Validate(result => result.resolvedSecond.IntProperty.Should().NotBe(42, "The value should not be set on the second instance"));

        [TestMethod]
        public void RegisterFactoryMethodAsNonSingleInstance_NewInstanceCreatedForEachResolve() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterFactoryMethod(c => new InternalDependency1()).As<IInternalDependency1>();
                return containerBuilder.Build();
            })
            .Act(container =>
            {
                var resolvedFirst = container.Resolve<IInternalDependency1>();
                resolvedFirst.IntProperty = 42;
                var resolvedSecond = container.Resolve<IInternalDependency1>();

                return (resolvedFirst, resolvedSecond);
            })
            .Assert()
                .Validate(result => result.resolvedFirst.Should().NotBeSameAs(result.resolvedSecond, "The two resolved instances should be different"))
                .Validate(result => result.resolvedSecond.IntProperty.Should().NotBe(42, "The value should not be set on the second instance"));

        [TestMethod]
        public void RegisterTypeAsSingleInstance_SameInstanceReturnedForEachResolve() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>().AsSingleInstance();
                return containerBuilder.Build();
            })
            .Act(container =>
            {
                var resolvedFirst = container.Resolve<IInternalDependency1>();
                resolvedFirst.IntProperty = 42;
                var resolvedSecond = container.Resolve<IInternalDependency1>();

                return (resolvedFirst, resolvedSecond);
            })
            .Assert()
                .Validate(result => result.resolvedFirst.Should().BeSameAs(result.resolvedSecond, "The two resolved instances should be the same"))
                .Validate(result => result.resolvedSecond.IntProperty.Should().Be(42, "The value should be set on the single instance"));

        [TestMethod]
        public void RegisterFactoryMethodAsSingleInstance_SameInstanceReturnedForEachResolve() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterFactoryMethod(c => new InternalDependency1()).As<IInternalDependency1>().AsSingleInstance();
                return containerBuilder.Build();
            })
            .Act(container =>
            {
                var resolvedFirst = container.Resolve<IInternalDependency1>();
                resolvedFirst.IntProperty = 42;
                var resolvedSecond = container.Resolve<IInternalDependency1>();

                return (resolvedFirst, resolvedSecond);
            })
            .Assert()
                .Validate(result => result.resolvedFirst.Should().BeSameAs(result.resolvedSecond, "The two resolved instances should be the same"))
                .Validate(result => result.resolvedSecond.IntProperty.Should().Be(42, "The value should be set on the single instance"));

        [TestMethod]
        public void SameTypeRegisterWithSameAliasTwiceInOneStep_OnlyOneIsKeptAndResolved() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>().As<IInternalDependency1>();
                return containerBuilder.Build();
            })
            .Act(container => container.ResolveAll<IInternalDependency1>().ToList())
            .Assert()
                .Validate(result => result.Count.Should().Be(1, "The same registration should not be resolved as different instances"));

        [TestMethod]
        public void SameTypeRegisterWithSameAliasTwiceInDifferentSteps_OnlyOneIsKeptAndResolved() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>();
                containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>();
                return containerBuilder.Build();
            })
            .Act(container => container.ResolveAll<IInternalDependency1>().ToList())
            .Assert()
                .Validate(result => result.Count.Should().Be(1, "The same registration should not be resolved as different instances"));


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