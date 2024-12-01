using AuthDbExtension.Interfaces;
using AuthDbExtension;
using Microsoft.AspNetCore.Mvc;
using Blazorik.WebApi.ModelsDTO;

namespace Blazorik.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAccessControl _accessControl;
        public AuthController(IAccessControl accessControl)
        {
            _accessControl = accessControl;
        }

        // POST: api/Auth
        // BODY: (JSON)
        [HttpPost("Registration")]
        [ProducesResponseType(201, Type = typeof(UserDTO))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Registration([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ArgumentNullException($"User cannot be null");
            }

            var user = new User
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                PasswordHash = userDTO.PasswordHash,
            };

            var result = await _accessControl.RegistrationAsync(user);
            return Ok(user);
        }

        [HttpPost("Login")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginResponse)
        {
            if (string.IsNullOrEmpty(loginResponse.Email) || string.IsNullOrEmpty(loginResponse.Password))
            {
                throw new ArgumentException($"email or password cannot be null");
            }
            var result = await _accessControl.LoginAsync(loginResponse.Email, loginResponse.Password);
            return Ok(result);
        }
    }
}
