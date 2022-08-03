using System;
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
    }
}