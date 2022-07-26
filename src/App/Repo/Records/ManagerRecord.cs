using System;

namespace IntrepidProducts.Repo.Records
{
    public class ManagerRecord : IRecord
    {
        public Guid ManagerPersonId { get; set; }
        public Guid DirectReportPersonId { get; set; }
    }
}