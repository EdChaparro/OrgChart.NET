using System.Linq;
using IntrepidProducts.Repo.Records;
using Microsoft.EntityFrameworkCore;

namespace IntrepidProducts.Repo
{
    public static class DbContextExtensions
    {
        public static bool DetachLocal<TRecord>
            (this DbContext dbContext, TRecord record, EntityState state)
            where TRecord : class, IRecord
        {
            var local = dbContext.Set<TRecord>().Local
                .FirstOrDefault(x => x.Equals(record));

            if (local == null)
            {
                return false;
            }

            dbContext.Entry(local).State = EntityState.Detached;
            dbContext.Entry(record).State = state;
            return true;
        }
    }
}