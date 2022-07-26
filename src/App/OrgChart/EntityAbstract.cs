using System;

namespace IntrepidProducts.OrgChart
{
    public interface IHasId
    {
        Guid Id { get; }
    }

    public abstract class EntityAbstract : IHasId
    {
        protected EntityAbstract() : this(Guid.NewGuid())
        {}

        protected EntityAbstract(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Entity ID is invalid");
            }

            Id = id;
        }

        public Guid Id { get; }

        public virtual bool IsValid()
        {
            return true;
        }

        #region Equality
        public override bool Equals(object obj)
        {
            return Id.Equals(obj);
        }

        protected bool Equals(EntityAbstract other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion
    }
}