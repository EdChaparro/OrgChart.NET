using System;
using System.Linq;
using IntrepidProducts.Repo;
using IntrepidProducts.Repo.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntrepidProducts.RepoTests.Entities
{
    [TestClass]
    public class PersonRepoTest
    {
        [TestMethod]
        public void ShouldSupportCreate()
        {
            var repo = new PersonRepo();

            var person = new Person(Guid.NewGuid())
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var count = repo.Create(person);

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ShouldNotCreateWhenEntityNotValid()
        {
            var repo = new PersonRepo();

            var person = new Person(Guid.NewGuid())
            {
                FirstName = "John", //No Last Name
            };

            var count = repo.Create(person);

            Assert.AreEqual(0, count);  //Nothing created
        }

        [TestMethod]
        public void ShouldPersistManager()
        {
            var repo = new PersonRepo();

            var manager = new Person
            {
                FirstName = "Dave",
                LastName = "Smith"
            };

            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                ReportsTo = manager
            };

            var count = repo.Create(person);
            Assert.AreEqual(1, count);

            var personFromDB = repo.FindById(person.Id);
            Assert.IsNotNull(personFromDB);

            Assert.IsTrue(personFromDB.IsManaged);
            Assert.AreEqual(manager.Id, personFromDB.ReportsTo.Id);
        }

        [TestMethod]
        public void ShouldPermitExistingPersonToManage()
        {
            var repo = new PersonRepo();

            var manager = new Person
            {
                FirstName = "Dave",
                LastName = "Smith"
            };

            Assert.AreEqual(1, repo.Create(manager));


            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                ReportsTo = manager
            };

            Assert.AreEqual(1, repo.Create(person));

            var personFromDB = repo.FindById(person.Id);
            Assert.IsNotNull(personFromDB);
            Assert.IsTrue(personFromDB.IsManaged);
            Assert.AreEqual(manager.Id, personFromDB.ReportsTo.Id);
        }

        [TestMethod]
        public void ShouldPersistDirectReports()
        {
            var repo = new PersonRepo();

            var manager = new Person
            {
                FirstName = "Dave",
                LastName = "Smith"
            };

            var directReport = new Person
            {
                FirstName = "John",
                LastName = "Doe",
            };

            manager.AddDirectReport(directReport);

            var count = repo.Create(manager);
            Assert.AreEqual(1, count);

            var managerFromDB = repo.FindById(manager.Id);
            Assert.IsNotNull(managerFromDB);
            Assert.AreEqual(1, managerFromDB.DirectReportCount);
            Assert.AreEqual(directReport.Id, managerFromDB.DirectReports.First().Id);

            var directReportFromDB = repo.FindById(directReport.Id);
            Assert.IsNotNull(directReportFromDB);
            Assert.IsTrue(directReportFromDB.IsManaged);
            Assert.AreEqual(directReportFromDB.ReportsTo, managerFromDB);
        }
    }
}