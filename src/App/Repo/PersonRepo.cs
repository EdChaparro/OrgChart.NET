using System;
using IntrepidProducts.Repo.Entities;

namespace IntrepidProducts.Repo
{
    public interface IPersonRepo : IRepository<Person>
    {
        Person? FindById(Guid id);
    }

    public class PersonRepo : RepoAbstract<Person, PersonContext>, IPersonRepo
    {
        public Person? FindById(Guid id)
        {
            return DbContext.Find(new Person(id));
        }
    }
}