using ProductsStore.DAL;
using ProductsStore.Models.Users;

namespace ProductsStore.WebAPI.Providers
{
    public class UserProvider
    {
        private readonly DataBaseContext _dataBaseContext;
        public UserProvider(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        //public async Task<User> AddUser(User user)
        //{
        //    var res = await _dataBaseContext.Users.AddAsync(user);
        //    await _dataBaseContext.SaveChangesAsync();
        //    User addedUser = res.Entity;
        //    return addedUser;
        //}

        //public async Task<bool> IsUserExist(User user)
        //{
        //    return await _dataBaseContext.Users.AnyAsync(u => u.Id == user.Id);
        //}

        //public async Task<User> GetUser(string userName)
        //{
        //    User addedUser = await _dataBaseContext.Users
        //                        .Where(un => un.UserName == userName)
        //                        .Include(un => un.Person)
        //                        .FirstAsync();
        //    return addedUser;
        //}

        //public async Task<Person> SavePerson(Person person)
        //{
        //    Person? savePerson = await _dataBaseContext.Persons
        //        .Where(un => un.Id == person.Id)
        //        .FirstAsync();
        //    if (savePerson != null)
        //    {
        //        savePerson.FirstName = person.FirstName;
        //        savePerson.LastName = person.LastName;
        //        savePerson.Patronymic = person.Patronymic;
        //        savePerson.Address = person.Address;
        //        savePerson.Phone = person.Phone;
        //        await _dataBaseContext.SaveChangesAsync();
        //        return savePerson;
        //    }
        //    throw new Exception("Не удалось сохранить данные");
        //}
    }
}
