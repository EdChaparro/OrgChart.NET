using IntrepidProducts.Repo.Records;
using Microsoft.EntityFrameworkCore;

namespace IntrepidProducts.Repo
{
    public interface IDbContext<TEntity> where TEntity : class
    {
        int Create(TEntity entity);
        int Update(TEntity entity);
        int Delete(TEntity entity);
        TEntity? Find(TEntity entity);
    }

    public abstract class DbContextAbstract<TEntity, TRecord> : DbContext, IDbContext<TEntity>
        where TEntity : class, IEntity
        where TRecord : class, IRecord, new()
    {
        protected DbContextAbstract(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        { }

        protected abstract TRecord Convert(TEntity entity);
        protected abstract TEntity Convert(TRecord record);

        protected virtual string DatabaseName => "Database";

        protected DbSet<TRecord>? DbSet { get; set; }

        public virtual int Create(TEntity entity)
        {
            DbSet.Add(Convert(entity));
            return SaveChanges();
        }

        public virtual int Update(TEntity entity)
        {
            var item = Find(entity);
            if (item == null)
            {
                return 0;
            }

            var isDetached = DbContextExtensions.DetachLocal
                (this, Convert(entity), EntityState.Modified);

            if (!isDetached)
            {
                return 0;
            }

            //DbSet.Update(Convert(entity)); //Removed due to EF tracking issues while testing
            return SaveChanges();
        }

        public abstract TEntity? Find(TEntity entity);

        public virtual int Delete(TEntity entity)
        {
            var item = Find(entity);
            if (item == null)
            {
                return 0;
            }

            var isDetached = DbContextExtensions.DetachLocal
                (this, Convert(entity), EntityState.Deleted);

            if (!isDetached)
            {
                return 0;
            }

            //DbSet.Remove(Convert(entity));    //Not needed, already flagged for deletion
            return SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(DatabaseName);
            }
        }
    }
}