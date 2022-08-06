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
    }
}