using DiLite.Builders;
using DiLite.Exceptions;
using DiLite.Tests.Classes;
using DiLite.Tests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace DiLite.Tests
{
    [TestClass]
    public class TestRegistrationErrors
    {
        [TestMethod]
        public void BadSelfRegistration_RegisterTypeThrowsException()
        {
            Exception resultException = null;
            var containerBuilder = new ContainerBuilder();

            try
            {
                containerBuilder.RegisterType<Main1>().As<Main1>();
            }
            catch (Exception ex)
            {
                resultException = ex;
            }

            Assert.IsNotNull(resultException,
                "Registration should throw an exception");

            Assert.IsInstanceOfType(resultException, typeof(InvalidOperationException),
                "The specified kind of exception should be thrown");
        }

        [TestMethod]
        public void BadInterfaceRegistration_RegisterTypeThrowsException()
        {
            Exception resultException = null;
            var containerBuilder = new ContainerBuilder();

            try
            {
                containerBuilder.RegisterType<Main1>().As<IMain2>();
            }
            catch (Exception ex)
            {
                resultException = ex;
            }

            Assert.IsNotNull(resultException,
                "Registration should throw an exception");

            Assert.IsInstanceOfType(resultException, typeof(InvalidOperationException),
                "The specified kind of exception should be thrown");
        }

        [TestMethod]
        public void InterfaceAsInstanceRegistration_RegisterTypeThrowsException()
        {
            Exception resultException = null;
            var containerBuilder = new ContainerBuilder();

            try
            {
                containerBuilder.RegisterType<IInternalDependency1>();
            }
            catch (Exception ex)
            {
                resultException = ex;
            }

            Assert.IsNotNull(resultException,
                "Registration should throw an exception");

            Assert.IsInstanceOfType(resultException, typeof(NotInstantiableTypeException),
                "The specified kind of exception should be thrown");
        }

        [TestMethod]
        public void AbstractClassAsInstanceRegistration_RegisterTypeThrowsException()
        {
            Exception resultException = null;
            var containerBuilder = new ContainerBuilder();

            try
            {
                containerBuilder.RegisterType<AbstractClass>();
            }
            catch (Exception ex)
            {
                resultException = ex;
            }

            Assert.IsNotNull(resultException,
                "Registration should throw an exception");

            Assert.IsInstanceOfType(resultException, typeof(NotInstantiableTypeException),
                "The specified kind of exception should be thrown");
        }
    }
}