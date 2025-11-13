using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsStore.WebAPI.DTO;
using ProductsStore.WebAPI.Providers.Interface;
using ProductsStore.WebAPI.Service.Interface;
using System.Security.Claims;

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
                        role = user.Role.Title
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _authProvider.GetUserByUsernameAsync(dto.UserName);
                if (user == null)
                    return Unauthorized(new { message = "Invalid username or password" });

                var passwordValid = _authService.VerifyPassword(dto.Password, user.PasswordHash);
                if (!passwordValid)
                    return Unauthorized(new { message = "Invalid username or password" });

                var token = _authService.GenerateJwtToken(user);

                return Ok(new
                {
                    userId = user.Id,
                    token,
                    role = user.Role.Title
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при входе", error = ex.Message });
            }
        }

        
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {    
                var username = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrWhiteSpace(username))
                    return Unauthorized();

                var user = await _authProvider.GetUserByUsernameAsync(username);
                if (user == null)
                    return NotFound();

                return Ok(new
                {
                    userId = user.Id,
                    userName = user.UserName,
                    role = user.Role?.Title,
                    person = user.Person == null ? null : new
                    {
                        id = user.Person.Id,
                        lastName = user.Person.LastName,
                        firstName = user.Person.FirstName,
                        patronymic = user.Person.Patronymic,
                        address = user.Person.Address,
                        phone = user.Person.Phone
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении данных пользователя", error = ex.Message });
            }
        }
    }
}
