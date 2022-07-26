
using System;

namespace IntrepidProducts.Repo.Records
{
    public class PersonRecord : IRecord, IHasId
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}