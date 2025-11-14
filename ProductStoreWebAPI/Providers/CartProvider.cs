using Microsoft.EntityFrameworkCore;
using ProductsStore.DAL;
using ProductsStore.Models.Carts;
using ProductsStore.Models.Products;
using ProductsStore.Models.Users;
using ProductsStore.WebAPI.DTO.Cart;

namespace ProductsStore.WebAPI.Providers
{
    public class CartProvider
    {
        private readonly DataBaseContext _dataBaseContext;
        public CartProvider(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        public async Task<Cart> GetCart(Guid userID)
        {
            var cart = await _dataBaseContext.Carts
                        .Where(un => un.UserID == userID)
                        .Include(un => un.Products)
                        .ThenInclude(p=>p.Category)
                        .FirstOrDefaultAsync();
            if (cart != null)
            {
                return cart;
            }

            var user = await _dataBaseContext.Users
                                             .Where(un => un.Id == userID)
                                             .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("Пользователь не найден");
            }
            var newCart = await _dataBaseContext.Carts.AddAsync(new Cart
            {
                User = user,
                Products = new List<Product>()
            });

            await _dataBaseContext.SaveChangesAsync();
            return newCart.Entity;
        }

        public async Task<Cart> AddToCart(User user, Guid id)
        {
            try
            {
                Cart savedCart = await this.GetCart(user.Id);

                Product savedProduct = await _dataBaseContext.Products
                                                             .Where(un => un.Id == id)
                                                             .FirstAsync();

                if (savedCart.Products.Any(un => un.Id == savedProduct.Id))
                {
                    return savedCart;
                }

                savedCart.Products.Add(savedProduct);
                await _dataBaseContext.SaveChangesAsync();
                return savedCart;
            }
            catch
            {
                throw new Exception("Не удалось добавить товар в корзину");
            }
        }

        public async Task<Cart> DeleteFromCart(User user, Guid id)
        {
            try
            {
                Cart savedCart = await this.GetCart(user.Id);

                Product savedProduct = await _dataBaseContext.Products
                                                             .Where(un => un.Id == id)
                                                             .FirstAsync();
                if (savedCart.Products.Any(un => un.Id == savedProduct.Id))
                {
                    savedCart.Products.Remove(savedProduct);
                    await _dataBaseContext.SaveChangesAsync();
                    return savedCart;
                }
                return savedCart;
            }
            catch
            {
                throw new Exception("Не удалось удалить товар из корзины");
            }
        }

        public async Task<Cart> ClearCart(User user)
        {
            var cart = await this.GetCart(user.Id);

            cart.Products.Clear();
            await _dataBaseContext.SaveChangesAsync();
            return cart;
        }
    }
}
