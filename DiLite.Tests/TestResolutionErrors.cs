using DiLite.Builders;
using DiLite.Exceptions;
using DiLite.Tests.Classes;
using DiLite.Tests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestLite;

namespace DiLite.Tests
{
    [TestClass]
    public class TestResolutionErrors
    {
        [TestMethod]
        public void ResolveNotRegisteredAlias_ResolveThrowsException() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>();
                return containerBuilder.Build();
            })
            .Act(container => container.Resolve<IInternalDependency2>())
            .Assert().ThrewException<NotRegisteredException>("Resolving a not registered alias should throw an exception");

        [TestMethod]
        public void ResolveNotRegisteredAlias_ResolveAllThrowsException() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>();
                return containerBuilder.Build();
            })
            .Act(container => container.ResolveAll<IInternalDependency2>())
            .Assert().ThrewException<NotRegisteredException>("Resolving a not registered alias should throw an exception");

        [TestMethod]
        public void ResolveClassWithNotRegisteredDependency_ResolveThrowsException() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<Dependency1>().As<IDependency1>().AsSelf();
                containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>().AsSelf();
                containerBuilder.RegisterType<InternalDependency2>().As<IInternalDependency2>().AsSelf();
                // Missing registration of dependency 'InternalDependency3'.
                return containerBuilder.Build();
            })
            .Act(container => container.Resolve<IDependency1>())
            .Assert().ThrewException<DependencyResolutionException>("Resolving a class with not registered dependency should throw an exception");
        // TODO:
        //Assert.IsNotNull(resultException.InnerException, "The thrown exception should contain an InnerException");
        //Assert.IsInstanceOfType(resultException.InnerException, typeof(NotRegisteredException), "The InnerException should be of the specified kind");
        //Assert.IsTrue(resultException.InnerException.Message.Contains(typeof(IInternalDependency3).FullName), "The message of the InnerException should contain the name of the missing dependency");

        [TestMethod]
        public void ResolveClassWithMultiplePublicConstructors_ResolveThrowsException() => Test
            .Arrange(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>();
                containerBuilder.RegisterType<InternalDependency2>().As<IInternalDependency2>();
                containerBuilder.RegisterType<ClassWithMultiplePublicConstructors>();
                return containerBuilder.Build();
            })
            .Act(container => container.Resolve<ClassWithMultiplePublicConstructors>())
            .Assert().ThrewException<ConstructorException>("Resolving a class with multiple public constructors should throw an exception");
        // TODO:
        //Assert.IsTrue(resultException.Message.Contains(typeof(ClassWithMultiplePublicConstructors).FullName), "The message of the exception should contain the name of the type with multiple public constructors");

        [TestMethod]
        public void ResolveFactoryMethodThatThrowsException_ResolveThrowsException() => Test
            .Arrange(() =>
            {
                var factoryMethodException = new DivideByZeroException("This is an error.");

                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterFactoryMethod<Main1>(context => throw factoryMethodException).As<IMain1>();
                return containerBuilder.Build();
            })
            .Act(container => container.Resolve<IMain1>())
            .Assert().ThrewException<InstanceCreationFailedException>("Resolving a class with multiple public constructors should throw an exception");
        // TODO:
        //Assert.IsNotNull(resultException.InnerException, "The inner exception must not be null");
        //Assert.IsInstanceOfType(resultException.InnerException, factoryMethodException.GetType(), "The inner exception must be the same type as the exception thrown by the factory method");
        //Assert.AreEqual(resultException.InnerException.Message, factoryMethodException.Message, "The inner exception must contain the message of the exception thrown by the factory method");
    }
}