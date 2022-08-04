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
        #region Create
        [TestMethod]
        public void ShouldSupportPersonCreate()
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
        public void ShouldNotCreatePersonWhenInvalid()
        {
            var repo = new PersonRepo();

            var person = new Person(Guid.NewGuid())
            {
                FirstName = "John", //No Last Name
            };

            var count = repo.Create(person);

            Assert.AreEqual(0, count);  //Nothing created
        }

        #region Manager
        [TestMethod]
        public void ShouldPersistManager()
        {
            var repo = new PersonRepo();

            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Clerk"
            };
            repo.Create(person);

            var manager = new Person
            {
                FirstName = "Dave",
                LastName = "Smith",
                Title = "Vice President"
            };
            Assert.AreEqual(1, repo.Create(manager));

            Assert.IsNull(repo.FindManager(person.Id));

            Assert.AreEqual(1, repo.PersistDirectReports(manager, person.Id));
            Assert.AreEqual(manager, repo.FindManager(person.Id));
        }

        [TestMethod]
        public void ShouldPersistManagerChange()
        {
            var repo = new PersonRepo();

            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Clerk"
            };
            Assert.AreEqual(1, repo.Create(person));

            var manager = new Person
            {
                FirstName = "Dave",
                LastName = "Smith",
                Title = "Vice President"
            };
            Assert.AreEqual(1, repo.Create(manager));

            Assert.AreEqual(1, repo.PersistDirectReports(manager, person.Id));
            Assert.AreEqual(manager, repo.FindManager(person.Id));

            var newManager = new Person
            {
                FirstName = "Foo",
                LastName = "Bar",
                Title = "Chief Executive Officer"
            };
            Assert.AreEqual(1, repo.Create(newManager));

            Assert.AreEqual(1, repo.PersistDirectReports(newManager, person.Id));
            Assert.AreEqual(newManager, repo.FindManager(person.Id));

            Assert.IsFalse(repo.FindDirectReports(manager.Id).Any());
        }

        [TestMethod]
        public void ShouldNotPermitPersonToAssignThemselvesAsManager()
        {
            var repo = new PersonRepo();

            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Clerk"
            };
            Assert.AreEqual(1, repo.Create(person));

            Assert.AreEqual(0, repo.PersistDirectReports(person, person.Id));
            Assert.IsNull(repo.FindManager(person.Id));
        }

        [TestMethod]
        public void ShouldPersistDirectReports()
        {
            var repo = new PersonRepo();

            var manager = new Person(Guid.NewGuid())
            {
                FirstName = "Dave",
                LastName = "Smith",
                Title = "Vice President"
            };
            Assert.AreEqual(1, repo.Create(manager));

            var dr1 = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Clerk"
            };
            repo.Create(dr1);

            var dr2 = new Person
            {
                FirstName = "Foo",
                LastName = "Bar",
                Title = "Clerk"
            };
            repo.Create(dr2);

            Assert.IsNull(repo.FindManager(dr1.Id));
            Assert.IsNull(repo.FindManager(dr2.Id));

            Assert.AreEqual(2, repo.PersistDirectReports(manager, dr1.Id, dr2.Id));
            Assert.AreEqual(manager, repo.FindManager(dr1.Id));
            Assert.AreEqual(manager, repo.FindManager(dr2.Id));
        }
        #endregion
        #endregion

        [TestMethod]
        public void ShouldFindById()
        {
            var repo = new PersonRepo();

            var person = new Person(Guid.NewGuid())
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var count = repo.Create(person);
            Assert.AreEqual(1, count);

            Assert.AreEqual(person, repo.FindById(person.Id));
        }
    }
}