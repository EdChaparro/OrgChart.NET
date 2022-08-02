using System;

namespace IntrepidProducts.Repo.Records
{
    public interface IHasId
    {
        Guid Id { get; }
    }
}