using Microsoft.AspNetCore.Mvc;
using ProductsStore.WebAPI.DTO;
using ProductsStore.WebAPI.Providers;
using ProductsStore.WebAPI.Providers.Interface;
using ProductsStore.WebAPI.Service;
using ProductsStore.WebAPI.Service.Interface;

namespace ProductsStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthProvider _authProvider;
        private readonly IAuthService _authService;

        public AuthController(IAuthProvider authProvider, IAuthService authService)
        {
            _authProvider = authProvider;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Хэшируем пароль
                dto.Password = _authService.HashPassword(dto.Password);

                // Регистрируем пользователя
                var user = await _authProvider.RegisterUserAsync(dto);

                // Генерируем токен
                var token = _authService.GenerateJwtToken(user);

                return CreatedAtAction(
                    nameof(Register),
                    new { id = user.Id },
                    new
                    {
                        userId = user.Id,
                        token,
                        role = user.UserRole.Title
                    });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при регистрации", error = ex.Message });
            }
        }
    }
}
