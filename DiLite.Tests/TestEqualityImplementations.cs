using DiLite.Registrations;
using DiLite.Tests.Classes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestLite;

namespace DiLite.Tests
{
    [TestClass]
    public class TestEqualityImplementations
    {
        [TestMethod]
        public void RegisteredEntitiesWithSameType_TypeSpecificEqualityCheckReturnsTrue() => Test
            .Arrange(() => (new RegisteredType(typeof(Main1), false), new RegisteredType(typeof(Main1), false)))
            .Act((registeredEntity1, registeredEntity2) => registeredEntity1.Equals(registeredEntity2))
            .Assert()
                .Validate(result => result.Should().BeTrue("Registered entities with the same type should be equal"));

        [TestMethod]
        public void RegisteredEntitiesWithDifferentType_TypeSpecificEqualityCheckReturnsFalse() => Test
            .Arrange(() => (new RegisteredType(typeof(Main1), false), new RegisteredType(typeof(Main2), false)))
            .Act((registeredEntity1, registeredEntity2) => registeredEntity1.Equals(registeredEntity2))
            .Assert()
                .Validate(result => result.Should().BeFalse("Registered entities with different type should not be equal"));

        [TestMethod]
        public void RegisteredEntityAndNull_TypeSpecificEqualityCheckReturnsFalse() => Test
            .Arrange(() => new RegisteredType(typeof(Main1), false))
            .Act(registeredEntity1 => registeredEntity1.Equals(null))
            .Assert()
                .Validate(result => result.Should().BeFalse("Registered entity and null should not be equal"));

        [TestMethod]
        public void RegisteredEntitiesWithSameType_BoxedEqualityCheckReturnsTrue() => Test
            .Arrange(() => (new RegisteredType(typeof(Main1), false), (object)new RegisteredType(typeof(Main1), false)))
            .Act((registeredEntity1, registeredEntity2) => registeredEntity1.Equals(registeredEntity2))
            .Assert()
                .Validate(result => result.Should().BeTrue("Registered entities with the same type should be equal"));

        [TestMethod]
        public void RegisteredEntitiesWithDifferentType_BoxedEqualityCheckReturnsFalse() => Test
            .Arrange(() => (new RegisteredType(typeof(Main1), false), (object)new RegisteredType(typeof(Main2), false)))
            .Act((registeredEntity1, registeredEntity2) => registeredEntity1.Equals(registeredEntity2))
            .Assert()
                .Validate(result => result.Should().BeFalse("Registered entities with different type should not be equal"));

        [TestMethod]
        public void RegisteredEntityAndNull_BoxedEqualityCheckReturnsFalse() => Test
            .Arrange(() => new RegisteredType(typeof(Main1), false))
            .Act(registeredEntity1 => registeredEntity1.Equals((object)null))
            .Assert()
                .Validate(result => result.Should().BeFalse("Registered entity and null should not be equal"));
    }
}