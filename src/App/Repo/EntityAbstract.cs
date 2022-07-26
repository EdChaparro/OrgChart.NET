﻿using System;

namespace IntrepidProducts.Repo
{
    public interface IEntity : IHasId
    { }

    public abstract class EntityAbstract : IEntity
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
        public override bool Equals(object? obj)
        {
            var otherEntity = obj as EntityAbstract;

            return otherEntity != null && otherEntity.Id == Id;
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