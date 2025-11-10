using ProductsStore.DAL;
using ProductsStore.Models.Carts;
using ProductsStore.Models.Orders;
using ProductsStore.Models.Products;

namespace ProductsStore.WebAPI.Providers
{
    public class OrderProvider
    {
        private readonly DataBaseContext _dataBaseContext;
        public OrderProvider(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        //public async Task<bool> SaveOrder(Guid cartId)
        //{
        //    Cart savedCart = await _dataBaseContext.Carts
        //                              .Where(un => un.Id == cartId)
        //                              .FirstAsync();
        //    Order savedOrder = new Order
        //    {
        //        User = savedCart.User,
        //        Products = new List<Product>(savedCart.Products),
        //    };
        //    await _dataBaseContext.Orders.AddAsync(savedOrder);
        //    await _dataBaseContext.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<List<Order>> GetOrders(Guid userID)
        //{
        //    var res = _dataBaseContext.Orders
        //                        .Where(un => un.UserID == userID).Include(un => un.Products);

        //    return await res.ToListAsync();
        //}
    }
}
