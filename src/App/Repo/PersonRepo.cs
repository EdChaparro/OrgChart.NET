using System;
using System.Collections.Generic;
using IntrepidProducts.Repo.Entities;
using IntrepidProducts.Repo.Records;

namespace IntrepidProducts.Repo
{
    public interface IPersonRepo : IRepository<Person>
    {
        Person? FindById(Guid id);
        ManagerRecord? FindManager(Guid directReportId);
        IEnumerable<Person> FindDirectReports(Guid managerId);
        bool PersistDirectReport(Person manager, Guid directReportId);
    }

    public class PersonRepo : RepoAbstract<Person, PersonContext>, IPersonRepo
    {
        public Person? FindById(Guid id)
        {
            return DbContext.Find(new Person(id));
        }

        public ManagerRecord? FindManager(Guid directReportId)
        {
            return DbContext.FindManager(directReportId);
        }

        public IEnumerable<Person> FindDirectReports(Guid managerId)
        {
            return DbContext.FindDirectReports(managerId);
        }

        public bool PersistDirectReport(Person manager, Guid directReportId)
        {
            return DbContext.PersistDirectReport(manager, directReportId);
        }
    }
}