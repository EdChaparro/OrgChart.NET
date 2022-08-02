
using System;
using IntrepidProducts.Repo.Entities;

namespace IntrepidProducts.Repo.Records
{
    public class PersonRecord : IRecord, IHasId
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public static PersonRecord Convert(Person entity)
        {
            return new PersonRecord
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName
            };
        }

        public static Person Convert(PersonRecord record)
        {
            return new Person(record.Id)
            {
                FirstName = record.FirstName,
                LastName = record.LastName
            };
        }

        public override string ToString()
        {
            return $"{FullName}, {Id}";
        }
    }
}