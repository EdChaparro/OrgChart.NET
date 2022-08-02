using IntrepidProducts.Repo.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntrepidProducts.RepoTests.Entities
{
    [TestClass]
    public class PersonTest
    {
        #region Validation
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
        #endregion

        [TestMethod]
        public void ShouldProperlyFormatFullName()
        {
            var p = new Person { FirstName = "John", LastName = "Doe" };

            Assert.AreEqual("John Doe", p.FullName);
        }

        [TestMethod]
        public void ShouldMaintainDirectReportCount()
        {
            var p = new Person { FirstName = "John", LastName = "Doe" };
            Assert.AreEqual(0, p.DirectReportCount);

            var dr = new Person { FirstName = "Dave", LastName = "Smith" };

            p.AddDirectReport(dr);
            Assert.AreEqual(1, p.DirectReportCount);
        }
    }
}