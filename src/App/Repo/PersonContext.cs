using System;
using System.Collections.Generic;
using System.Linq;
using IntrepidProducts.Repo.Entities;
using IntrepidProducts.Repo.Records;
using Microsoft.EntityFrameworkCore;

namespace IntrepidProducts.Repo
{
    public interface IPersonContext : IDbContext<Person>
    {}

    public class PersonContext : DbContextAbstract<Person, PersonRecord>, IPersonContext
    {
        public PersonContext()
            : base(new DbContextOptions<PersonContext>())
        {}

        private DbSet<ManagerRecord> ManagerDbSet { get; set; }

        protected override PersonRecord Convert(Person entity)
        {
            return PersonRecord.Convert(entity);
        }

        protected override Person Convert(PersonRecord record)
        {
            return PersonRecord.Convert(record);
        }

        public override int Create(Person entity)
        {
            if (!entity.IsValid())
            {
                return 0;
            }

            var createResult = base.Create(entity);

            if (createResult < 1)
            {
                return 0;
            }

            var managerCreateResult = PersistManager(entity);

            var directReportsCreateResult = PersistDirectReports(entity);

            return 1;
        }

        private bool PersistManager(Person entity)
        {
            if (!entity.IsManaged)
            {
                return true;
            }

            var managerRecord = ManagerRecord.Convert(entity.ReportsTo, entity);

            if (managerRecord == null)
            {
                return false;
            }

            var managerPerson = Find(new Person(managerRecord.ManagerPersonId));
            if (managerPerson == null)
            {
                managerPerson = new Person(managerRecord.ManagerPersonId)
                {
                    FirstName = entity.FirstName,
                    LastName = entity.LastName
                };

                var managerCreateResult =  Create(managerPerson);

               if (managerCreateResult < 1)
               {
                   return false;
               }
            }

            ManagerDbSet.Add(managerRecord);
            var result = SaveChanges();

            return result > 0;
        }

        private bool PersistDirectReports(Person entity)
        {
            if (!entity.IsManager)
            {
                return true;
            }

            var directReportsAdded = 0;

            foreach (var dr in entity.DirectReports)
            {
                var drPerson = Find(new Person(dr.Id));

                if (drPerson == null)
                {
                    var drCreateResult = Create(dr);

                    if (drCreateResult < 1)
                    {
                        return false;
                    }
                }

                var managerRecord = ManagerRecord.Convert(entity, dr);

                ManagerDbSet.Add(managerRecord);
                directReportsAdded++;
            }

            if (directReportsAdded > 0)
            {
                var result = SaveChanges();
                return result > 0;
            }

            return false;
        }

        public override Person? Find(Person entity)
        {
            return FindById(entity.Id);
        }

        private Person? FindById(Guid id)
        {
            var record = DbSet.Find(id);

            if (record == null)
            {
                return null;
            }

            var person = Convert(record);

            person.ReportsTo = FindManager(person);

            var directReports = FindDirectReports(person);

            foreach (var dr in directReports)
            {
                person.AddDirectReport(dr);
            }

            return person;
        }

        private Person? FindManager(Person person)
        {
            var managerRecord = ManagerDbSet
                .FirstOrDefault(x => x.DirectReportPersonId == person.Id);

            if (managerRecord != null)
            {
                return Find(new Person(managerRecord.ManagerPersonId));
            }

            return null;
        }

        private IEnumerable<Person> FindDirectReports(Person manager)
        {
            var directReports = new List<Person>();

            var directReportRecords = ManagerDbSet
                .Where(x => x.ManagerPersonId == manager.Id);

            foreach (var directReportRecord in directReportRecords)
            {
                var directReport = DbSet.Find(directReportRecord.DirectReportPersonId);

                if (directReport != null)
                {
                    directReports.Add(
                        new Person(directReport.Id)
                        {
                            FirstName = directReport.FirstName,
                            LastName = directReport.LastName,
                            ReportsTo = manager
                        });
                }
            }

            return directReports;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ManagerRecord>()
                .HasKey(
                    nameof(ManagerRecord.ManagerPersonId),
                    nameof(ManagerRecord.DirectReportPersonId));
        }
    }
}