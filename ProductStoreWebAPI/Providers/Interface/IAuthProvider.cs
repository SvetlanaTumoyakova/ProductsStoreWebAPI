using ProductsStore.Models.Users;
using ProductsStore.WebAPI.DTO;

namespace ProductsStore.WebAPI.Providers.Interface
{
    public interface IAuthProvider
    {
        Task<User> RegisterUserAsync(RegisterDto dto);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> IsUsernameTakenAsync(string username);
    }
}
