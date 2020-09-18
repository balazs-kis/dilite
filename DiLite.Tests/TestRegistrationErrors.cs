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
    public class TestRegistrationErrors
    {
        [TestMethod]
        public void BadSelfRegistration_RegisterTypeThrowsException() => Test
            .Arrange(() => new ContainerBuilder())
            .Act(containerBuilder => containerBuilder.RegisterType<Main1>().As<Main1>())
            .Assert().ThrewException<InvalidOperationException>("Bad self-registration should throw an exception");

        [TestMethod]
        public void BadInterfaceRegistration_RegisterTypeThrowsException() => Test
            .Arrange(() => new ContainerBuilder())
            .Act(containerBuilder => containerBuilder.RegisterType<Main1>().As<IMain2>())
            .Assert().ThrewException<InvalidOperationException>("Bad interface registration should throw an exception");

        [TestMethod]
        public void InterfaceAsInstanceRegistration_RegisterTypeThrowsException() => Test
            .Arrange(() => new ContainerBuilder())
            .Act(containerBuilder => containerBuilder.RegisterType<IInternalDependency1>())
            .Assert().ThrewException<NotInstantiableTypeException>("Interface as instance registration should throw an exception");

        [TestMethod]
        public void AbstractClassAsInstanceRegistration_RegisterTypeThrowsException() => Test
            .Arrange(() => new ContainerBuilder())
            .Act(containerBuilder => containerBuilder.RegisterType<AbstractClass>())
            .Assert().ThrewException<NotInstantiableTypeException>("Abstract class as instance registration should throw an exception");

        [TestMethod]
        public void CallingBuildTwice_BuilderThrowsException() => Test
            .Arrange(() => new ContainerBuilder())
            .Act(containerBuilder =>
            {
                containerBuilder.RegisterType<InternalDependency1>().AsSelf();
                containerBuilder.RegisterType<InternalDependency2>().AsSelf();

                containerBuilder.Build();
                containerBuilder.Build();
            })
            .Assert().ThrewException<InvalidOperationException>("Calling build twice should throw an exception");
    }
}