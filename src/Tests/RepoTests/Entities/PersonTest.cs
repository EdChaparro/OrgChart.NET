using IntrepidProducts.Repo.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntrepidProducts.RepoTests.Entities
{
    [TestClass]
    public class PersonTest
    {
        [TestMethod]
        public void ShouldFailValidationWhenFirstNameMissing()
        {
            var p = new Person { LastName = "Doe" };

            Assert.IsFalse(p.IsValid());
        }

        [TestMethod]
        public void ShouldFailValidationWhenLastNameMissing()
        {
            var p = new Person { FirstName = "John" };

            Assert.IsFalse(p.IsValid());
        }

        [TestMethod]
        public void ShouldPassValidationWhenFirstAndLastNameProvided()
        {
            var p = new Person { FirstName = "John", LastName = "Doe" };

            Assert.IsTrue(p.IsValid());
        }

        [TestMethod]
        public void ShouldProperlyFormatFullName()
        {
            var p = new Person { FirstName = "John", LastName = "Doe" };

            Assert.AreEqual("John Doe", p.FullName);
        }
    }
}