using System;
using System.Collections.Generic;
using System.Linq;
using IntrepidProducts.Repo.Entities;

namespace IntrepidProducts.Repo
{
    public interface IPersonRepo : IRepository<Person>
    {
        Person? FindById(Guid id);
        Person? FindManager(Guid directReportId);
        IEnumerable<Person> FindDirectReports(Guid managerId);
        int PersistDirectReports(Person manager, params Guid[] directReportIds);

        bool RemoveManager(Guid directReportId);
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

        public override bool Delete(Person person)
        {
            if (FindDirectReports(person.Id).Any())
            {
                return false;   //Direct reports must be reassigned before deletion
            }

            return base.Delete(person);
        }

        private IEnumerable<Person> FindChainOfCommand(Guid directReportId)
        {
            var chainOfCommand = new List<Person>();

            var nextPersonId = directReportId;

            while (true)
            {
                var manager = FindManager(nextPersonId);
                if (manager == null)
                {
                    break;
                }

                chainOfCommand.Add(manager);
                nextPersonId = manager.Id;
            }

            return chainOfCommand;
        }

        private bool IsDirectReportRelationshipValid(Guid managerId, Guid directReportId)
        {
            return IsRelationshipReferenceValid(managerId, directReportId) &&
                   IsDirectReportEligible(managerId, directReportId);
        }

        private bool IsDirectReportEligible(Guid managerId, Guid directReportId)
        {
            var chainOfCommand = FindChainOfCommand(managerId);

            return chainOfCommand.All(x => x.Id != directReportId);
        }

        private bool IsRelationshipReferenceValid(Guid managerId, Guid directReportId)
        {
            if (managerId == directReportId)
            {
                return false;
            }

            var persistedManager = FindById(managerId);
            var directReport = FindById(directReportId);

            return persistedManager != null && directReport != null;
        }

        public int PersistDirectReports(Person manager, params Guid[] directReportIds)
        {
            var count = 0;

            foreach (var id in directReportIds)
            {
                if (!IsDirectReportRelationshipValid(manager.Id, id))
                {
                    continue;
                }

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

        public bool RemoveManager(Guid directReportId)
        {
            var managerRecord = DbContext.FindManager(directReportId);
            if (managerRecord == null)
            {
                return false;
            }

            return DbContext.RemoveManager(directReportId);
        }
    }
}