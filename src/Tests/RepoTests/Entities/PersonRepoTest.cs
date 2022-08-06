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

            Assert.IsTrue(repo.Create(person));
        }

        [TestMethod]
        public void ShouldNotCreatePersonWhenInvalid()
        {
            var repo = new PersonRepo();

            var person = new Person(Guid.NewGuid())
            {
                FirstName = "John", //No Last Name
            };

            Assert.IsFalse(repo.Create(person));    //Nothing created
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
            Assert.IsTrue(repo.Create(manager));

            Assert.IsNull(repo.FindManager(person.Id));

            Assert.AreEqual(1, repo.PersistDirectReports(manager.Id, person.Id));
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
            Assert.IsTrue(repo.Create(person));

            var manager = new Person
            {
                FirstName = "Dave",
                LastName = "Smith",
                Title = "Vice President"
            };
            Assert.IsTrue(repo.Create(manager));

            Assert.AreEqual(1, repo.PersistDirectReports(manager.Id, person.Id));
            Assert.AreEqual(manager, repo.FindManager(person.Id));

            var newManager = new Person
            {
                FirstName = "Foo",
                LastName = "Bar",
                Title = "Chief Executive Officer"
            };
            Assert.IsTrue(repo.Create(newManager));

            Assert.AreEqual(1, repo.PersistDirectReports(newManager.Id, person.Id));
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
            Assert.IsTrue(repo.Create(person));

            Assert.AreEqual(0, repo.PersistDirectReports(person.Id, person.Id));
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
            Assert.IsTrue(repo.Create(manager));

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

            Assert.AreEqual(2, repo.PersistDirectReports(manager.Id, dr1.Id, dr2.Id));
            Assert.AreEqual(manager, repo.FindManager(dr1.Id));
            Assert.AreEqual(manager, repo.FindManager(dr2.Id));
        }

        [TestMethod]
        public void ShouldNotPersistChainOfCommandMembersAsDirectReports()
        {
            var repo = new PersonRepo();

            var manager = new Person(Guid.NewGuid())
            {
                FirstName = "Dave",
                LastName = "Smith",
                Title = "Assistant Manager"
            };
            Assert.IsTrue(repo.Create(manager));

            var director = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Director"
            };
            repo.Create(director);

            var svp = new Person
            {
                FirstName = "Foo",
                LastName = "Bar",
                Title = "Senior Vice President"
            };
            repo.Create(svp);

            var dr1 = new Person
            {
                FirstName = "Bruno",
                LastName = "Brazer",
                Title = "Clerk"
            };
            repo.Create(dr1);

            var dr2 = new Person
            {
                FirstName = "Sally",
                LastName = "Mae",
                Title = "Clerk"
            };
            repo.Create(dr2);

            Assert.AreEqual(2, repo.PersistDirectReports(manager.Id, dr1.Id, dr2.Id));
            Assert.AreEqual(1, repo.PersistDirectReports(director.Id, manager.Id));
            Assert.AreEqual(1, repo.PersistDirectReports(svp.Id, director.Id));

            Assert.AreEqual(0, repo.PersistDirectReports(dr2.Id, director.Id));
            Assert.AreEqual(0, repo.PersistDirectReports(dr1.Id, manager.Id));
            Assert.AreEqual(0, repo.PersistDirectReports(dr1.Id, svp.Id));
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

            Assert.IsTrue(repo.Create(person));
            Assert.AreEqual(person, repo.FindById(person.Id));
        }
    }
}