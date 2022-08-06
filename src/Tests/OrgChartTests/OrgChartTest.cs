using IntrepidProducts.Repo.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntrepidProducts.OrgChart.Tests
{
    [TestClass]
    public class OrgChartTest
    {
        [TestMethod]
        public void ShouldMaintainDirectReportCount()
        {
            var orgChart = new OrgChart(new Person { FirstName = "John", LastName = "Doe" });
            Assert.AreEqual(0, orgChart.DirectReportCount);

            var dr1 = new OrgChart(new Person { FirstName = "Dave", LastName = "Smith" });
            var dr2 = new OrgChart(new Person { FirstName = "John", LastName = "Doe" });

            orgChart.AddDirectReport(dr1, dr2);
            Assert.AreEqual(2, orgChart.DirectReportCount);
        }

        [TestMethod]
        public void ShouldCalculateChartDepth()
        {
            var ceo = new OrgChart(new Person
            {
                FirstName = "Tyler",
                LastName = "James",
                Title = "Chief Executive Officer"
            });

            var svp = new OrgChart(new Person
            {
                FirstName = "Jesse",
                LastName = "Owens",
                Title = "Senior Vice President"
            });

            var director = new OrgChart(new Person
            {
                FirstName = "Clark",
                LastName = "Kent",
                Title = "Director"
            });

            var manager1 = new OrgChart(new Person
            {
                FirstName = "Dave",
                LastName = "Smith",
                Title = "Manager"
            });

            var manager2 = new OrgChart(new Person
            {
                FirstName = "Frank",
                LastName = "Stallone",
                Title = "Senior Manager"
            });

            var dr1 = new OrgChart(new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Title = "Clerk"
            });

            var dr2 = new OrgChart(new Person
            {
                FirstName = "Foo",
                LastName = "Bar",
                Title = "Inventory Clerk"
            });

            var dr3 = new OrgChart(new Person
            {
                FirstName = "Bat",
                LastName = "Man",
                Title = "Operations Lead"
            });

            Assert.IsTrue(ceo.AddDirectReport(svp));
            Assert.IsTrue(svp.AddDirectReport(director));
            Assert.IsTrue(director.AddDirectReport(manager1, manager2));
            Assert.IsTrue(manager1.AddDirectReport(dr1, dr2));
            Assert.IsTrue(manager2.AddDirectReport(dr3));

            Assert.AreEqual(1, dr1.NumberOfLevels);
            Assert.AreEqual(1, dr2.NumberOfLevels);
            Assert.AreEqual(1, dr3.NumberOfLevels);

            Assert.AreEqual(2, manager1.NumberOfLevels);
            Assert.AreEqual(2, manager2.NumberOfLevels);

            Assert.AreEqual(3, director.NumberOfLevels);
            Assert.AreEqual(4, svp.NumberOfLevels);
            Assert.AreEqual(5, ceo.NumberOfLevels);
        }
    }
}