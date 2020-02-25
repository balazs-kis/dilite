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
        public void TestNotRegisteredResolution()
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

            Assert.IsNotNull(resultException);
            Assert.IsInstanceOfType(resultException, typeof(NotRegisteredException));
        }

        [TestMethod]
        public void TestDependencyNotRegisteredResolution()
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

            Assert.IsNotNull(resultException);
            Assert.IsInstanceOfType(resultException, typeof(DependencyResolutionException));
            Assert.IsNotNull(resultException.InnerException);
            Assert.IsInstanceOfType(resultException.InnerException, typeof(NotRegisteredException));
            Assert.IsTrue(resultException.InnerException.Message.Contains(typeof(IInternalDependency3).FullName));
        }
    }
}