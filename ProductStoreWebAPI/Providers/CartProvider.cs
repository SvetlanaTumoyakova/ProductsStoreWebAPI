using ProductsStoreWebAPI.Context;
using ProductStoreWebAPI.Model.Carts;
using ProductStoreWebAPI.Model.Products;

namespace ProductStoreWebAPI.Providers
{
    public class CartProvider
    {
        private readonly ProductsStoreContext _productsStoreContext;
        public CartProvider(ProductsStoreContext productsStoreContext)
        {
            _productsStoreContext = dataBaseContext;
        }
        public async Task<Cart> AddToCart(AddToCartDto cartDto)
        {
            try
            {
                Cart savedCart = await _productsStoreContext.Carts
                                       .Where(un => un.Id == cartDto.CartId)
                                       .FirstAsync();
                Product savedProduct = await _productsStoreContext.Products
                                                             .Where(un => un.Id == cartDto.ProductId)
                                                             .FirstAsync();
                if (savedCart.Products.Any(un => un.Id == savedProduct.Id))
                {
                    return savedCart;
                }

                savedCart.Products.Add(savedProduct);
                await _productsStoreContext.SaveChangesAsync();
                return savedCart;
            }
            catch
            {
                throw new Exception("Не удалось добавить товар в корзину");
            }
        }

        public async Task<Cart> DeleteFromCart(DeleteFromCartDto cartDto)
        {
            try
            {
                Cart savedCart = await _productsStoreContext.Carts
                                       .Where(un => un.Id == cartDto.CartId)
                                       .FirstAsync();
                Product savedProduct = await _productsStoreContext.Products
                                                             .Where(un => un.Id == cartDto.ProductId)
                                                             .FirstAsync();
                if (savedCart.Products.Any(un => un.Id == savedProduct.Id))
                {
                    savedCart.Products.Remove(savedProduct);
                    await _productsStoreContext.SaveChangesAsync();
                    return savedCart;
                }
                return savedCart;
            }
            catch
            {
                throw new Exception("Не удалось удалить товар из корзины");
            }
        }

        public async Task<Cart> GetCart(Guid userID)
        {
            var cart = await _productsStoreContext.Carts
                        .Where(un => un.UserID == userID)
                        .Include(un => un.Products)
                        .FirstOrDefaultAsync();
            if (cart != null)
            {
                return cart;
            }

            var user = await _productsStoreContext.Users
                                             .Where(un => un.Id == userID)
                                             .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("Пользователь не найден");
            }
            var newCart = await _productsStoreContext.Carts.AddAsync(new Cart
            {
                User = user,
                Products = new List<Product>()
            });

            await _productsStoreContext.SaveChangesAsync();
            return newCart.Entity;
        }

        public async Task<Cart> ClearCart(Guid cartId)
        {
            var cart = await _productsStoreContext.Carts
                         .Where(un => un.Id == cartId)
                         .Include(un => un.Products)
                         .FirstOrDefaultAsync();
            if (cart == null)
            {
                throw new Exception("Корзина не найдена");
            }

            cart.Products.Clear();
            await _productsStoreContext.SaveChangesAsync();
            return cart;
        }
    }
}
