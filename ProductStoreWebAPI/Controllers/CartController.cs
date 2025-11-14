using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsStore.WebAPI.DTO.Cart;
using ProductsStore.WebAPI.Providers;
using ProductsStore.WebAPI.Providers.Interface;
using System.Security.Claims;

namespace ProductsStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly CartProvider _cartProvider;
        private readonly IAuthProvider _authProvider;

        public CartController(CartProvider cartProvider, IAuthProvider authProvider)
        {
            _cartProvider = cartProvider;
            _authProvider = authProvider;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            var username = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized();

            var user = await _authProvider.GetUserByUsernameAsync(username);
            if (user == null)
                return Unauthorized();

            var cart = await _cartProvider.GetCart(user.Id);
            return Ok(cart);
        }

        [HttpPost("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> AddToCart(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var username = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized();

            var user = await _authProvider.GetUserByUsernameAsync(username);
            if (user == null)
                return Unauthorized();

            try
            {
                var cart = await _cartProvider.AddToCart(user, id);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var username = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized();

            var user = await _authProvider.GetUserByUsernameAsync(username);
            if (user == null)
                return Unauthorized();

            try
            {
                var cart = await _cartProvider.DeleteFromCart(user, id);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("clear")]
        [Authorize]
        public async Task<IActionResult> ClearCart()
        {
            var username = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized();

            var user = await _authProvider.GetUserByUsernameAsync(username);
            if (user == null)
                return Unauthorized();

            try
            {
                var cart = await _cartProvider.ClearCart(user);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
