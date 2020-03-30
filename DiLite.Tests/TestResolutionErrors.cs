using DiLite.Builders;
using DiLite.Exceptions;
using DiLite.Tests.Classes;
using DiLite.Tests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiLite.Tests
{
    [TestClass]
    public class TestResolutionErrors
    {
        [TestMethod]
        public void ResolveNotRegisteredAlias_ResolveThrowsException()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>();
            var container = containerBuilder.Build();

            Exception resultException = null;
            try
            {
                container.Resolve<IInternalDependency2>();
            }
            catch (Exception ex)
            {
                resultException = ex;
            }

            Assert.IsNotNull(resultException,
                "Resolution should throw an exception");

            Assert.IsInstanceOfType(resultException, typeof(NotRegisteredException),
                "The specified kind of exception should be thrown");
        }

        [TestMethod]
        public void ResolveNotRegisteredAlias_ResolveAllThrowsException()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>();
            var container = containerBuilder.Build();

            Exception resultException = null;
            try
            {
                container.ResolveAll<IInternalDependency2>();
            }
            catch (Exception ex)
            {
                resultException = ex;
            }

            Assert.IsNotNull(resultException,
                "Resolution should throw an exception");

            Assert.IsInstanceOfType(resultException, typeof(NotRegisteredException),
                "The specified kind of exception should be thrown");
        }

        [TestMethod]
        public void ResolveClassWithNotRegisteredDependency_ResolveThrowsException()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Dependency1>().As<IDependency1>().AsSelf();
            containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>().AsSelf();
            containerBuilder.RegisterType<InternalDependency2>().As<IInternalDependency2>().AsSelf();
            // Missing registration of dependency 'InternalDependency3'.
            var container = containerBuilder.Build();

            Exception resultException = null;
            try
            {
                container.Resolve<IDependency1>();
            }
            catch (Exception ex)
            {
                resultException = ex;
            }

            Assert.IsNotNull(resultException,
                "Resolution should throw an exception");

            Assert.IsInstanceOfType(resultException, typeof(DependencyResolutionException),
                "The specified kind of exception should be thrown");

            Assert.IsNotNull(resultException.InnerException,
                "The thrown exception should contain an InnerException");

            Assert.IsInstanceOfType(resultException.InnerException, typeof(NotRegisteredException),
                "The InnerException should be of the specified kind");

            Assert.IsTrue(resultException.InnerException.Message.Contains(typeof(IInternalDependency3).FullName),
                "The message of the InnerException should contain the name of the missing dependency");
        }

        [TestMethod]
        public void ResolveClassWithMultiplePublicConstructors_ResolveThrowsException()
        {
            Exception resultException = null;
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<InternalDependency1>().As<IInternalDependency1>();
            containerBuilder.RegisterType<InternalDependency2>().As<IInternalDependency2>();
            containerBuilder.RegisterType<ClassWithMultiplePublicConstructors>();
            var container = containerBuilder.Build();

            try
            {
                container.Resolve<ClassWithMultiplePublicConstructors>();
            }
            catch (Exception ex)
            {
                resultException = ex;
            }

            Assert.IsNotNull(resultException,
                "Resolution should throw an exception");

            Assert.IsInstanceOfType(resultException, typeof(ConstructorException),
                "The specified kind of exception should be thrown");

            Assert.IsTrue(resultException.Message.Contains(typeof(ClassWithMultiplePublicConstructors).FullName),
                "The message of the exception should contain the name of the type with multiple public constructors");
        }

        [TestMethod]
        public void ResolveFactoryMethodThatThrowsException_ResolveThrowsException()
        {
            Exception resultException = null;
            var factoryMethodException = new DivideByZeroException("This is an error.");

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterFactoryMethod<Main1>(context => throw factoryMethodException).As<IMain1>();
            var container = containerBuilder.Build();

            try
            {
                container.Resolve<IMain1>();
            }
            catch (Exception ex)
            {
                resultException = ex;
            }

            Assert.IsNotNull(resultException,
                "Resolution should throw an exception");

            Assert.IsInstanceOfType(resultException, typeof(InstanceCreationFailedException),
                "The specified kind of exception should be thrown");

            Assert.IsNotNull(resultException.InnerException,
                "The inner exception must not be null");

            Assert.IsInstanceOfType(resultException.InnerException, factoryMethodException.GetType(),
                "The inner exception must be the same type as the exception thrown by the factory method");

            Assert.AreEqual(resultException.InnerException.Message, factoryMethodException.Message,
                "The inner exception must contain the message of the exception thrown by the factory method");
        }
    }
}