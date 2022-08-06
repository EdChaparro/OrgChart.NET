using System.Collections.Generic;
using System.Linq;
using IntrepidProducts.Repo.Entities;

namespace IntrepidProducts.OrgChart
{
    public class OrgChart
    {
        public OrgChart(Person person)
        {
            ForPerson = person;
        }

        public Person ForPerson { get; }

        public int NumberOfLevels => GetDirectReportDepth(this);

        public Person? ReportsTo { get; set; }

        #region Direct Reports
        private readonly List<OrgChart> _directReports = new List<OrgChart>();
        public IEnumerable<OrgChart> DirectReports => _directReports;
        public bool IsManager => DirectReports.Any();

        public int DirectReportCount => DirectReports.Count();

        private static int GetDirectReportDepth(OrgChart orgChart)
        {
            var depth = 1;
            var drDepth = 0;

            foreach (var directReport in orgChart.DirectReports.ToList())
            {
                var nodeDepth = GetDirectReportDepth(directReport);

                if (nodeDepth > drDepth)
                {
                    drDepth = nodeDepth;
                }
            }

            return depth + drDepth;
        }

        public bool AddDirectReport(params OrgChart[] orgCharts)
        {
            foreach (var orgChart in orgCharts)
            {
                if (!AddDirectReport(orgChart))
                {
                    return false;
                }
            }

            return true;
        }

        private bool AddDirectReport(OrgChart orgChart)
        {
            if (_directReports.Contains(orgChart))
            {
                return false;
            }

            _directReports.Add(orgChart);
            return true;
        }
        #endregion

        #region Equality
        public override bool Equals(object? obj)
        {
            var otherEntity = obj as OrgChart;

            return otherEntity != null && otherEntity.ForPerson.Id == ForPerson.Id;
        }

        protected bool Equals(OrgChart other)
        {
            return ForPerson.Equals(other.ForPerson);
        }

        public override int GetHashCode()
        {
            return ForPerson.Id.GetHashCode();
        }
        #endregion


        public override string ToString()
        {
            return $"Org Chart for {ForPerson}, {NumberOfLevels} levels";
        }
    }
}