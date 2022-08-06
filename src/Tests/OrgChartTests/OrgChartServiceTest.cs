using System.Collections.Generic;
using System.Linq;
using IntrepidProducts.Repo;
using IntrepidProducts.Repo.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntrepidProducts.OrgChart.Tests
{
    [TestClass]
    public class OrgChartServiceTest
    {
        [TestMethod]
        public void ShouldAddManagerAndDirectReports()
        {
            IOrgChartService service = new OrgChartService(new PersonRepo());

            var manager = new Person
            {
                FirstName = "Dave",
                LastName = "Smith",
                Title = "Manager"
            };

            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Clerk"
            };

            var count = service.Add(manager, person);
            Assert.AreEqual(2, count);

            service.AddDirectReports(manager.Id, person.Id);

            var orgChart = service.GetOrgChartFor(manager.Id);
            Assert.IsNotNull(orgChart);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.AreEqual(manager, orgChart.ForPerson);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            Assert.IsTrue(orgChart.IsManager);
            Assert.AreEqual(new OrgChart(person), orgChart.DirectReports.First());
        }

        [TestMethod]
        public void ShouldUpdatePerson()
        {
            IOrgChartService service = new OrgChartService(new PersonRepo());

            const string ORIG_FIRST_NAME = "John";
            const string ORIG_LAST_NAME = "Doe";
            const string ORIG_TITLE = "Clerk";

            const string MODIFIED_FIRST_NAME = "Foo";
            const string MODIFIED_LAST_NAME = "Bar";
            const string MODIFIED_TITLE = "Senior Clerk";

            var person = new Person
            {
                FirstName = ORIG_FIRST_NAME,
                LastName = ORIG_LAST_NAME,
                Title = ORIG_TITLE
            };
            var count = service.Add(person);
            Assert.AreEqual(1, count);

            person = service.FindById(person.Id);
            Assert.IsNotNull(person);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            person.FirstName = MODIFIED_FIRST_NAME;
            person.LastName = MODIFIED_LAST_NAME;
            person.Title = MODIFIED_TITLE;
            Assert.IsTrue(service.Update(person));

            person = service.FindById(person.Id);
            Assert.AreEqual(MODIFIED_FIRST_NAME, person.FirstName);
            Assert.AreEqual(MODIFIED_LAST_NAME, person.LastName);
            Assert.AreEqual(MODIFIED_TITLE, person.Title);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        [TestMethod]
        public void ShouldDeletePerson()
        {
            IOrgChartService service = new OrgChartService(new PersonRepo());

            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Analyst"
            };

            var addCount = service.Add(person);
            Assert.AreEqual(1, addCount);

            person = service.FindById(person.Id);
            Assert.IsNotNull(person);

            var isDeleted = service.Delete(person);
            Assert.IsTrue(isDeleted);

            person = service.FindById(person.Id);
            Assert.IsNull(person);
        }

        [TestMethod]
        public void ShouldNotDeletePersonsWithDirectReports()
        {
            IOrgChartService service = new OrgChartService(new PersonRepo());

            var manager = new Person
            {
                FirstName = "Foo",
                LastName = "Bar",
                Title = "Manager"
            };
            Assert.AreEqual(1, service.Add(manager));

            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Analyst"
            };
            Assert.AreEqual(1, service.Add(person));

            Assert.AreEqual(1, service.AddDirectReports(manager.Id, person.Id));

            var isDeleted = service.Delete(manager);
            Assert.IsFalse(isDeleted);
        }

        [TestMethod]
        public void ShouldRemoveManager()
        {
            IOrgChartService service = new OrgChartService(new PersonRepo());

            var manager = new Person
            {
                FirstName = "Dave",
                LastName = "Smith",
                Title = "Manager"
            };

            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Clerk"
            };

            var count = service.Add(manager, person);
            Assert.AreEqual(2, count);

            service.AddDirectReports(manager.Id, person.Id);

            var orgChart = service.GetOrgChartFor(person.Id);
            Assert.IsNotNull(orgChart);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.AreEqual(manager, orgChart.ReportsTo);

            Assert.IsTrue(service.RemoveManager(person));

            orgChart = service.GetOrgChartFor(person.Id);
            Assert.IsNotNull(orgChart);
            Assert.IsNull(orgChart.ReportsTo);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        [TestMethod]
        public void ShouldReplaceManager()
        {
            IOrgChartService service = new OrgChartService(new PersonRepo());

            var presentManager = new Person
            {
                FirstName = "Dave",
                LastName = "Smith",
                Title = "Manager"
            };

            var newManager = new Person
            {
                FirstName = "Foo",
                LastName = "Bar",
                Title = "Manager"
            };

            var dr1 = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Clerk"
            };

            var dr2 = new Person
            {
                FirstName = "Timothy",
                LastName = "Dalton",
                Title = "Clerk"
            };

            var count = service.Add(presentManager, newManager, dr1, dr2);
            Assert.AreEqual(4, count);

            service.AddDirectReports(presentManager.Id, dr1.Id, dr2.Id);

            var orgChart = service.GetOrgChartFor(presentManager.Id);
            Assert.IsNotNull(orgChart);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            CollectionAssert.AreEqual
                (new List<OrgChart> {new OrgChart(dr1), new OrgChart(dr2)}, 
                    orgChart.DirectReports.ToList());

            Assert.IsTrue(service.ReplaceManager(presentManager.Id, newManager.Id));

            orgChart = service.GetOrgChartFor(presentManager.Id);
            Assert.IsNotNull(orgChart);
            Assert.IsFalse(orgChart.IsManager);

            orgChart = service.GetOrgChartFor(newManager.Id);
            Assert.IsNotNull(orgChart);
            Assert.IsTrue(orgChart.IsManager);
            CollectionAssert.AreEqual
                (new List<OrgChart> { new OrgChart(dr1), new OrgChart(dr2) },
                    orgChart.DirectReports.ToList());

#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
