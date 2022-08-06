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

        bool RemoveManager(Person person);
        bool ReplaceManager(Guid oldManagerId, Guid newManagerId);

        int AddDirectReports(Guid managerId, params Guid[] directReportPersonIds);

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
                if (_repo.Create(person))
                {
                    count++;
                }
            }

            return count;
        }
        public bool Update(Person person)
        {
            return _repo.Update(person);
        }

        public bool Delete(Person person)
        {
            return _repo.Delete(person);
        }

        public bool RemoveManager(Person directReport)
        {
            return _repo.RemoveManager(directReport.Id);
        }

        public bool ReplaceManager(Guid oldManagerId, Guid newManagerId)
        {
            var directReports = _repo.FindDirectReports(oldManagerId).ToList();
            if (!directReports.Any())
            {
                return false;
            }

            var isSuccessful = true;

            foreach (var directReport in directReports)
            {
                isSuccessful = RemoveManager(directReport);
                if (!isSuccessful)
                {
                    break;
                }

                isSuccessful = AddDirectReports(newManagerId, directReport.Id) > 0;

                if (!isSuccessful)
                {
                    break;
                }
            }

            return isSuccessful;
        }

        public int AddDirectReports(Guid managerId, params Guid[] directReportPersonIds)
        {
            var count = 0;

            foreach (var drIds in directReportPersonIds)
            {
                var numberPersisted = _repo.PersistDirectReports(managerId, drIds);

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

            var directReports = _repo.FindDirectReports(personId)
                .Select(x => new OrgChart(x));

            orgChart.AddDirectReport(directReports.ToArray());

            return orgChart;
        }

        public Person? FindById(Guid id)
        {
            return _repo.FindById(id);
        }
    }
}