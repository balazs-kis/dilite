using DiLite.Registrations;
using DiLite.Tests.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiLite.Tests
{
    [TestClass]
    public class TestEqualityImplementations
    {
        [TestMethod]
        public void RegisteredEntitiesWithSameType_TypeSpecificEqualityCheckReturnsTrue()
        {
            var registeredEntity1 = new RegisteredType(typeof(Main1), false);
            var registeredEntity2 = new RegisteredType(typeof(Main1), false);

            var result = registeredEntity1.Equals(registeredEntity2);

            Assert.IsTrue(result, "Registered entities with the same type should be equal");
        }

        [TestMethod]
        public void RegisteredEntitiesWithDifferentType_TypeSpecificEqualityCheckReturnsFalse()
        {
            var registeredEntity1 = new RegisteredType(typeof(Main1), false);
            var registeredEntity2 = new RegisteredType(typeof(Main2), false);

            var result = registeredEntity1.Equals(registeredEntity2);

            Assert.IsFalse(result, "Registered entities with different type should not be equal");
        }

        [TestMethod]
        public void RegisteredEntityAndNull_TypeSpecificEqualityCheckReturnsFalse()
        {
            var registeredEntity1 = new RegisteredType(typeof(Main1), false);

            var result = registeredEntity1.Equals(null);

            Assert.IsFalse(result, "Registered entity and null should not be equal");
        }

        [TestMethod]
        public void RegisteredEntitiesWithSameType_BoxedEqualityCheckReturnsTrue()
        {
            var registeredEntity1 = new RegisteredType(typeof(Main1), false);
            object registeredEntity2 = new RegisteredType(typeof(Main1), false);

            var result = registeredEntity1.Equals(registeredEntity2);

            Assert.IsTrue(result, "Registered entities with the same type should be equal");
        }

        [TestMethod]
        public void RegisteredEntitiesWithDifferentType_BoxedEqualityCheckReturnsFalse()
        {
            var registeredEntity1 = new RegisteredType(typeof(Main1), false);
            object registeredEntity2 = new RegisteredType(typeof(Main2), false);

            var result = registeredEntity1.Equals(registeredEntity2);

            Assert.IsFalse(result, "Registered entities with different type should not be equal");
        }

        [TestMethod]
        public void RegisteredEntityAndNull_BoxedEqualityCheckReturnsFalse()
        {
            var registeredEntity1 = new RegisteredType(typeof(Main1), false);

            var result = registeredEntity1.Equals((object)null);

            Assert.IsFalse(result, "Registered entity and null should not be equal");
        }
    }
}