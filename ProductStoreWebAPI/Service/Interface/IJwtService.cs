using ProductsStore.Models.Users;

namespace ProductsStore.WebAPI.Service.Interface
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}

