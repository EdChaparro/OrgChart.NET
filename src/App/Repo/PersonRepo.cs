using System;
using System.Collections.Generic;
using IntrepidProducts.Repo.Entities;

namespace IntrepidProducts.Repo
{
    public interface IPersonRepo : IRepository<Person>
    {
        Person? FindById(Guid id);
        Person? FindManager(Guid directReportId);
        IEnumerable<Person> FindDirectReports(Guid managerId);
        int PersistDirectReports(Person manager, params Guid[] directReportIds);
    }

    public class PersonRepo : RepoAbstract<Person, PersonContext>, IPersonRepo
    {
        public Person? FindById(Guid id)
        {
            return DbContext.Find(new Person(id));
        }

        public Person? FindManager(Guid directReportId)
        {
            var managerRecord =  DbContext.FindManager(directReportId);

            if (managerRecord == null)
            {
                return null;
            }

            return FindById(managerRecord.ManagerPersonId);
        }

        public IEnumerable<Person> FindDirectReports(Guid managerId)
        {
            return DbContext.FindDirectReports(managerId);
        }

        public int PersistDirectReports(Person manager, params Guid[] directReportIds)
        {
            var count = 0;

            foreach (var id in directReportIds)
            {
                var isSuccessful = PersistDirectReport(manager, id);

                if (isSuccessful)
                {
                    count++;
                }
            }

            return count;
        }

        private bool PersistDirectReport(Person manager, Guid directReportId)
        {
            return DbContext.PersistDirectReport(manager, directReportId);
        }
    }
}