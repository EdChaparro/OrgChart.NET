using System.Collections.Generic;
using IntrepidProducts.Repo.Entities;
using IntrepidProducts.Repo.Records;
using Microsoft.EntityFrameworkCore;

namespace IntrepidProducts.Repo
{
    public class PersonContext : DbContextAbstract<Person, PersonRecord>
    {
        public PersonContext()
            : base(new DbContextOptions<PersonContext>())
        { }

        protected override IEnumerable<PersonRecord> Convert(Person entity)
        {
            throw new System.NotImplementedException();
        }

        public override Person? Find(Person entity)
        {
            throw new System.NotImplementedException();
        }
    }
}