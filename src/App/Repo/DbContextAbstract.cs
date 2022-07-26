using System;
using IntrepidProducts.Repo.Entities;
using Microsoft.EntityFrameworkCore;

namespace IntrepidProducts.Repo
{
    public interface IDbContext<TEntity> where TEntity : class
    {
        int Create(TEntity entity);
        int Update(TEntity entity);
        int Delete(TEntity entity);
        TEntity? FindById(Guid id);
    }

    public abstract class DbContextAbstract<TEntity> : DbContext, IDbContext<TEntity>
        where TEntity : class, IEntity
    {
        protected DbContextAbstract(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        { }

        protected DbSet<TEntity> DbSet { get; set; }

        public int Create(TEntity entity)
        {
            DbSet.Add(entity);
            return SaveChanges();
        }

        public int Update(TEntity entity)
        {
            var item = FindById(entity.Id);
            if (item == null)
            {
                return 0;
            }

            var isDetached = DbContextExtensions.DetachLocal
                (this, entity, EntityState.Modified);

            if (!isDetached)
            {
                return 0;
            }

            //DbSet.Update(entity); //Removed due to EF tracking issues in WebApp
            return SaveChanges();
        }

        public TEntity? FindById(Guid id)
        {
            return DbSet.Find(id);
        }

        public int Delete(TEntity entity)
        {
            var item = FindById(entity.Id);
            if (item == null)
            {
                return 0;
            }

            DbSet.Remove(entity);
            return SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("Exercise");
            }
        }
    }
}