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

        public Person ForPerson { get; set; }

        public int NumberOfLevels { get; set; }

        public Person? ReportsTo { get; set; }

        #region Direct Reports
        private readonly List<Person> _directReports = new List<Person>();
        public IEnumerable<Person> DirectReports => _directReports;
        public bool IsManager => DirectReports.Any();

        public int DirectReportCount => DirectReports.Count();

        public bool AddDirectReport(params OrgChart[] persons)
        {
            foreach (var person in persons)
            {
                if (!AddDirectReport(person))
                {
                    return false;
                }
            }

            return true;
        }

        private bool AddDirectReport(Person person)
        {
            if (_directReports.Contains(person))
            {
                return false;
            }

            _directReports.Add(person);
            return true;
        }

        public bool RemoveDirectReport(Person person)
        {
            if (!_directReports.Contains(person))
            {
                return false;
            }

            return _directReports.Remove(person);
        }
        #endregion

        public override string ToString()
        {
            return $"Org Chart for {ForPerson}, {NumberOfLevels} levels";
        }
    }
}