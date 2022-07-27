using System;
using IntrepidProducts.Repo.Entities;

namespace IntrepidProducts.Repo.Records
{
    public class ManagerRecord : IRecord
    {
        public Guid ManagerPersonId { get; set; }
        public Guid DirectReportPersonId { get; set; }

        public static ManagerRecord? Convert(Person manager, Person directReport)
        {
            return new ManagerRecord
            {
                ManagerPersonId = manager.Id,
                DirectReportPersonId = directReport.Id
            };
        }
    }
}