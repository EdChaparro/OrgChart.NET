namespace IntrepidProducts.Repo
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        bool Create(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);
    }

    public abstract class RepoAbstract<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TDbContext : IDbContext<TEntity>, new()
    {
        internal RepoAbstract()
        {
            DbContext = new TDbContext();
        }

        internal TDbContext DbContext { get; }

        public virtual bool Create(TEntity entity)
        {
            return DbContext.Create(entity) > 0;
        }

        public virtual bool Update(TEntity entity)
        {
            return DbContext.Update(entity) > 0;
        }

        public virtual bool Delete(TEntity entity)
        {
            return DbContext.Delete(entity) > 0;
        }
    }
}