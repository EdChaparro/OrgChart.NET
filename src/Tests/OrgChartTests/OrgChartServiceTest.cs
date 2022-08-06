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

            service.AddDirectReports(manager, person.Id);

            var orgChart = service.GetOrgChartFor(manager.Id);
            Assert.IsNotNull(orgChart);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.AreEqual(manager, orgChart.ForPerson);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            Assert.IsTrue(orgChart.IsManager);
            Assert.AreEqual(person, orgChart.DirectReports.First());
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
    }
}
