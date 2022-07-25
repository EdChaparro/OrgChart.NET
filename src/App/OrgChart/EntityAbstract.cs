using System;

namespace IntrepidProducts.OrgChart
{
    public interface IHasId
    {
        Guid Id { get; }
    }

    public abstract class EntityAbstract : IHasId
    {
        protected EntityAbstract(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}