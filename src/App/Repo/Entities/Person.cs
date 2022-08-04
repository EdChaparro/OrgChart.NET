using System;

namespace IntrepidProducts.Repo.Entities
{
    public class Person : EntityAbstract
    {
        public Person()
        {}

        public Person(Guid id) : base(id)
        {}

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string? Title { get; set; }

        public override bool IsValid()
        {
            if (String.IsNullOrWhiteSpace(FirstName))
            {
                return false;
            }

            if (String.IsNullOrWhiteSpace(LastName))
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"{FullName}, {Title}, {Id}";
        }
    }
}