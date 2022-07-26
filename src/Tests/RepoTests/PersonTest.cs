using IntrepidProducts.Repo.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntrepidProducts.Repo.Tests
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
            var p = new Person { FirstName = "John", LastName = "Doe"};

            Assert.IsTrue(p.IsValid());
        }

        [TestMethod]
        public void ShouldProperlyFormatFullName()
        {
            var p = new Person { FirstName = "John", LastName = "Doe" };

            Assert.AreEqual("John Doe", p.FullName);
        }

        [TestMethod]
        public void ShouldFailValidationWhenManagerIsInvalid()
        {
            var manager = new Person { FirstName = "Dave" }; //No Last Name

            var p = new Person { FirstName = "John", LastName = "Doe" };
            Assert.IsTrue(p.IsValid());

            p.ReportsTo = manager;
            Assert.IsFalse(p.IsValid());

            manager.LastName = "Smith";
            Assert.IsTrue(p.IsValid());
        }

        [TestMethod]
        public void ShouldFailValidationWhenDirectReportsAreInvalid()
        {
            var dr1 = new Person { FirstName = "Dave" }; //No Last Name
            var dr2 = new Person { LastName = "Stewart" }; //No Last Name

            var p = new Person { FirstName = "John", LastName = "Doe" };
            Assert.IsTrue(p.IsValid());

            p.AddDirectReport(dr1, dr2);
            Assert.IsFalse(p.IsValid());

            dr1.LastName = "Smith";
            dr2.FirstName = "Thomas";

            Assert.IsTrue(p.IsValid());
        }

    }
}
