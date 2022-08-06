using System;
using System.Linq;
using IntrepidProducts.Repo;
using IntrepidProducts.Repo.Entities;

namespace IntrepidProducts.OrgChart
{
    public interface IOrgChartService
    {
        int Add(params Person[] persons);
        bool Update(Person person);
        bool Delete(Person person);

        bool RemoveManager(Person person, Guid managerId);
        int ReplaceManager(Guid oldManagerId, Guid newManagerId);

        int AddDirectReports(Person manager, params Person[] directReportPersons);

        int AddDirectReports(Person manager, params Guid[] directReportPersonIds);

        OrgChart? GetOrgChartFor(Guid personId, int numberOfLevels = 2);

        Person? FindById(Guid id);
    }

    public class OrgChartService : IOrgChartService
    {
        public OrgChartService(IPersonRepo repo)
        {
            _repo = repo;
        }

        private readonly IPersonRepo _repo;

        public int Add(params Person[] persons)
        {
            var count = 0;
            foreach (var person in persons)
            {
                count = count + _repo.Create(person);
            }

            return count;
        }
        public bool Update(Person person)
        {
            var result = _repo.Update(person);
            return result > 0;
        }

        public bool Delete(Person person)
        {
            throw new NotImplementedException();
        }

        public bool RemoveManager(Person person, Guid managerId)
        {
            throw new NotImplementedException();
        }

        public int ReplaceManager(Guid oldManagerId, Guid newManagerId)
        {
            throw new NotImplementedException();
        }

        public int AddDirectReports(Person manager, params Person[] directReportPersons)
        {
            return AddDirectReports(manager, directReportPersons.Select(x => x.Id).ToArray());
        }

        public int AddDirectReports(Person manager, params Guid[] directReportPersonIds)
        {
            var count = 0;

            foreach (var drIds in directReportPersonIds)
            {
                var numberPersisted = _repo.PersistDirectReports(manager, drIds);

                count = count + numberPersisted;
            }

            return count;
        }

        public OrgChart? GetOrgChartFor(Guid personId, int numberOfLevels = 2)
        {
            var person = _repo.FindById(personId);

            if (person == null)
            {
                return null;
            }

            var manager = _repo.FindManager(person.Id);

            var orgChart = new OrgChart(person)
            {
                ReportsTo = manager
            };

            var directReports = _repo.FindDirectReports(personId);

            orgChart.AddDirectReport(directReports.ToArray());

            return orgChart;
        }

        public Person? FindById(Guid id)
        {
            return _repo.FindById(id);
        }
    }
}