using System;

namespace IntrepidProducts.Repo.Records
{
    public class ManagerRecord : IRecord
    {
        public Guid ManagerPersonId { get; set; }
        public Guid DirectReportPersonId { get; set; }

        public override bool Equals(object? obj)
        {
            var record = obj as ManagerRecord;

            if (record == null)
            {
                return false;
            }

            return Equals(record);
        }

        protected bool Equals(ManagerRecord other)
        {
            return ManagerPersonId.Equals(other.ManagerPersonId) &&
                   DirectReportPersonId.Equals(other.DirectReportPersonId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ManagerPersonId, DirectReportPersonId);
        }
    }
}