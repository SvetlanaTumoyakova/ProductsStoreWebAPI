using Microsoft.EntityFrameworkCore;
using ProductsStore.DAL;
using ProductsStore.Models.Carts;
using ProductsStore.Models.Orders;
using ProductsStore.Models.Products;
using ProductsStore.Models.Users;

namespace ProductsStore.WebAPI.Providers
{
    public class OrderProvider
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly CartProvider _cartProvider;
        public OrderProvider(DataBaseContext dataBaseContext, CartProvider cartProvider)
        {
            _dataBaseContext = dataBaseContext;
            _cartProvider = cartProvider;
        }

        public async Task<bool> SaveOrder(User user)
        {
            Cart savedCart = await _cartProvider.GetCart(user.Id);

            if (savedCart.Products.Count == 0) 
            { 
                throw new Exception("Корзина пустая");
            }

            Order savedOrder = new Order
            {
                User = savedCart.User,
                Products = new List<Product>(savedCart.Products),
            };
            await _dataBaseContext.Orders.AddAsync(savedOrder);
            await _dataBaseContext.SaveChangesAsync();
            return true;

        }

        //с пагинацией
        public async Task<(List<Order> Orders, int TotalCount)> GetOrders(User user, int page = 1, int pageSize = 10)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (page <= 0) throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize));

            var query = _dataBaseContext.Orders
                                .Where(un => un.UserID == user.Id)
                                .Include(un => un.Products)
                                .OrderByDescending(un => un.CreatedAt)
                                .AsQueryable();

            var totalCount = await query.CountAsync();

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (orders, totalCount);
        }

        public async Task<Order?> GetOrderById(User user, Guid orderId)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (orderId == Guid.Empty) throw new ArgumentException("Order id must be provided", nameof(orderId));

            var order = await _dataBaseContext.Orders
                .Where(o => o.Id == orderId && o.UserID == user.Id)
                .Include(o => o.Products)
                .FirstOrDefaultAsync();

            return order;
        }
    }
}
