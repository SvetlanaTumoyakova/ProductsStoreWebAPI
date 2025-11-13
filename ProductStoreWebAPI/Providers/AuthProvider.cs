using Microsoft.EntityFrameworkCore;
using ProductsStore.DAL;
using ProductsStore.Models.Users;
using ProductsStore.WebAPI.DTO;
using ProductsStore.WebAPI.Providers.Interface;

namespace ProductsStore.WebAPI.Providers
{
    public class AuthProvider : IAuthProvider
    {
        private readonly DataBaseContext _context;

        public AuthProvider(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterUserAsync(RegisterDto dto)
        {
           
            if (await IsUsernameTakenAsync(dto.UserName))
                throw new InvalidOperationException("Пользователь с таким email уже существует");

            var defaultRole = await _context.UserRoles
                .FirstOrDefaultAsync(r => r.Title == "User");

            if (defaultRole == null)
            {
                defaultRole = new UserRole { Title = "User" };
                await _context.UserRoles.AddAsync(defaultRole);
            }

            var person = new Person
            {
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                Patronymic = dto.Patronymic,
                Address = dto.Address,
                Phone = dto.Phone
            };

            await _context.Persons.AddAsync(person);

            var user = new User
            {
                UserName = dto.UserName,
                PasswordHash = dto.Password,
                Person = person,
                Role = defaultRole
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Person)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }


        /// <summary>
        /// Проверяет, занят ли указанный email в системе.
        /// </summary>
        /// <param name="username">Email пользователя для проверки.</param>
        /// <returns>True, если email уже зарегистрирован; иначе false.</returns>
        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username);
        }
    }
}
