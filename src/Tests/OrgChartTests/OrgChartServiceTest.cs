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
        public void ShouldPersistManagerAndDirectReports()
        {
            var service = new OrgChartService(new PersonRepo());

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
    }
}
