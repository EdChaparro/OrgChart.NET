using System;

namespace IntrepidProducts.Repo
{
    public interface IHasId
    {
        Guid Id { get; }
    }
}