using DiLite.Builders;
using DiLite.Exceptions;
using DiLite.Tests.Classes;
using DiLite.Tests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiLite.Tests
{
    [TestClass]
    public class TestRegistrationErrors
    {
        [TestMethod]
        public void TestBadSelfRegistration()
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

            Assert.IsNotNull(resultException);
            Assert.IsInstanceOfType(resultException, typeof(InvalidOperationException));
        }

        [TestMethod]
        public void TestBadInterfaceRegistration()
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

            Assert.IsNotNull(resultException);
            Assert.IsInstanceOfType(resultException, typeof(InvalidOperationException));
        }

        [TestMethod]
        public void TestInterfaceAsInstanceRegistration()
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

            Assert.IsNotNull(resultException);
            Assert.IsInstanceOfType(resultException, typeof(NotInstantiableTypeException));
        }

        [TestMethod]
        public void TestAbstractClassAsInstanceRegistration()
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

            Assert.IsNotNull(resultException);
            Assert.IsInstanceOfType(resultException, typeof(NotInstantiableTypeException));
        }
    }
}