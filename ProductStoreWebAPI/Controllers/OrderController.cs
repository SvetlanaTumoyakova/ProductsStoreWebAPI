using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsStore.WebAPI.Providers;
using ProductsStore.WebAPI.Providers.Interface;
using System.Security.Claims;

namespace ProductsStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly OrderProvider _orderProvider;
        private readonly IAuthProvider _authProvider;
        private const int DefaultPageSize = 10;

        public OrderController(OrderProvider orderProvider, IAuthProvider authProvider)
        {
            _orderProvider = orderProvider;
            _authProvider = authProvider;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var username = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized();

            var user = await _authProvider.GetUserByUsernameAsync(username);
            if (user == null)
                return Unauthorized();

            try
            {
                var success = await _orderProvider.SaveOrder(user);
                if (!success)
                    return StatusCode(500, new { message = "Не удалось сохранить заказ" });

                return Ok(new { message = "Заказ успешно создан" });
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при создании заказа", error = ex.Message });
            }
         
        }

        // GET: api/order?page=1
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] int page = 1, int size = DefaultPageSize)
        {
            if (page <= 0) return BadRequest(new { message = "Page must be greater than 0" });

            var username = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized();

            var user = await _authProvider.GetUserByUsernameAsync(username);
            if (user == null)
                return Unauthorized();

            try
            {
                var (orders, totalCount) = await _orderProvider.GetOrders(user, page, size);

                var totalPages = (int)Math.Ceiling(totalCount / (double)size);

                return Ok(new
                {
                    page,
                    pageSize = DefaultPageSize,
                    totalCount,
                    totalPages,
                    items = orders
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении заказов", error = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            if (id == Guid.Empty) return BadRequest(new { message = "Invalid order id" });

            var username = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized();

            var user = await _authProvider.GetUserByUsernameAsync(username);
            if (user == null)
                return Unauthorized();

            try
            {
                var order = await _orderProvider.GetOrderById(user, id);
                if (order == null)
                    return NotFound();

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении заказа", error = ex.Message });
            }
        }
    }
}
