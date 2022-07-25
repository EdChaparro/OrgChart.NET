using System;

namespace IntrepidProducts.OrgChart
{
    public class Person : EntityAbstract
    {
        public Person(Guid id) : base(id)
        {}

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}