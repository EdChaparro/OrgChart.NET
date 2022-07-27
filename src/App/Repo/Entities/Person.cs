using System;
using System.Collections.Generic;
using System.Linq;

namespace IntrepidProducts.Repo.Entities
{
    public class Person : EntityAbstract
    {
        public Person()
        {}

        public Person(Guid id) : base(id)
        {}

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string? Title { get; set; }

        public Person? ReportsTo { get; set; }
        public bool IsManaged => ReportsTo != null;

        #region Direct Reports
        private readonly List<Person> _directReports = new List<Person>();
        public IEnumerable<Person> DirectReports => _directReports;
        public bool IsManager => DirectReports.Any();

        public int DirectReportCount => DirectReports.Count();

        public bool AddDirectReport(params Person[] persons)
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

        public override bool IsValid()
        {
            if (String.IsNullOrWhiteSpace(FirstName))
            {
                return false;
            }

            if (String.IsNullOrWhiteSpace(LastName))
            {
                return false;
            }

            foreach (var directReport in DirectReports)
            {
                if (!directReport.IsValid())
                {
                    return false;
                }
            }

            return ReportsTo == null || ReportsTo.IsValid();
        }

        public override string ToString()
        {
            return $"{FullName}, {Id}";
        }
    }
}