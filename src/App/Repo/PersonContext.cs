using System;
using System.Collections.Generic;
using System.Linq;
using IntrepidProducts.Repo.Entities;
using IntrepidProducts.Repo.Records;
using Microsoft.EntityFrameworkCore;

namespace IntrepidProducts.Repo
{
    public interface IPersonContext : IDbContext<Person>
    {
        bool PersistDirectReport(Person manager, Guid directReportId);
        ManagerRecord? FindManager(Guid directReportId);
        bool RemoveManager(Guid directReportId);
        IEnumerable<Person> FindDirectReports(Guid managerId);
    }

    public class PersonContext : DbContextAbstract<Person, PersonRecord>, IPersonContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PersonContext()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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

            return base.Create(entity);
        }

        public override Person? Find(Person entity)
        {
            return FindById(entity.Id);
        }

        public bool PersistDirectReport(Person manager, Guid directReportId)
        {
            var presentManagerRecord = FindManager(directReportId);
            if (presentManagerRecord != null)
            {
                if (presentManagerRecord.ManagerPersonId == manager.Id)
                {
                    return true;
                }

                ManagerDbSet.Remove(presentManagerRecord);
            }

            var newManagerRecord = new ManagerRecord
                { DirectReportPersonId = directReportId, ManagerPersonId = manager.Id };

            ManagerDbSet.Add(newManagerRecord);
            var result = SaveChanges();

            return result > 0;
        }

        private Person? FindById(Guid id)
        {
            var record = DbSet.Find(id);

            if (record == null)
            {
                return null;
            }

            return Convert(record);
        }

        public ManagerRecord? FindManager(Guid directReportId)
        {
            return ManagerDbSet
                .FirstOrDefault(x => x.DirectReportPersonId == directReportId);
        }

        private ManagerRecord? FindManager(Person person)
        {
            return FindManager(person.Id);
        }

        public bool RemoveManager(Guid directReportId)
        {
            var managerRecord = FindManager(directReportId);
            if (managerRecord == null)
            {
                return false;
            }

            var isDetached = DbContextExtensions.DetachLocal
                (this, managerRecord, EntityState.Deleted);

            if (!isDetached)
            {
                return false;
            }

            return SaveChanges() > 0;
        }

        public IEnumerable<Person> FindDirectReports(Guid  managerId)
        {
            var directReports = new List<Person>();

            var directReportRecords = ManagerDbSet
                .Where(x => x.ManagerPersonId == managerId);

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