using System;
using System.Collections.Generic;
using System.Linq;

namespace IntrepidProducts.OrgChart
{
    public class Person : EntityAbstract
    {
        public Person(Guid id) : base(id)
        {}

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Title { get; set; }

        public Person? ReportsTo { get; set; }
        public bool IsManaged => ReportsTo != null;

        #region Direct Reports
        private readonly List<Person> _directReports = new List<Person>();
        public IEnumerable<Person> DirectReports => _directReports;
        public bool IsManager => DirectReports.Any();

        public bool AddDirectReport(Person person)
        {
            //TODO: Check if Person is valid
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
    }
}