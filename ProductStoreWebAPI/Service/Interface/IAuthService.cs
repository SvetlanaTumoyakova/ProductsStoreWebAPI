using ProductsStore.Models.Users;

namespace ProductsStore.WebAPI.Service.Interface
{
    public interface IAuthService
    {
        string HashPassword(string password);
        string GenerateJwtToken(User user);
        bool VerifyPassword(string password, string hash);
    }
}
