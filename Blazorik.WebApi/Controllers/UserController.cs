using AuthDbExtension.Interfaces;
using AuthDbExtension;
using Microsoft.AspNetCore.Mvc;
using Blazorik.WebApi.ModelsDTO;

namespace Blazorik.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserManager _userManager;
        private readonly IUserRoleManager _userRoleManager;
        private readonly IAccessControl _accessControl;
        private readonly IRoleManager _roleManager;
        private readonly UserDbcontext _context;
        public UserController(ILogger<UserController> logger, IUserManager userManager,
            IAccessControl accessControl, IUserRoleManager userRoleManager,
            IRoleManager roleManager, UserDbcontext context)
        {
            _logger = logger;
            _userManager = userManager;
            _accessControl = accessControl;
            _roleManager = roleManager;
            _userRoleManager = userRoleManager;
            _context = context;
        }


        [HttpPost("Test")]
        public async Task<IActionResult> InitialData()
        {
            //var t = await _userManager.AddTestDateAsync();
            var t = await _userManager.GetFullUsers();
            return Ok(t);
        }

        // GET /user
        [HttpGet]
        public async Task<IEnumerable<User?>> Get()
        {

            var users = await _userManager.GetFullUsers();
            return users;
        }

        // GET: api/user/[id]
        [HttpGet("{userId:Guid}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET: api/user/[string]
        [HttpGet("{email}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(string email)
        {
            var user = await _userManager.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/user
        // BODY: User (JSON)
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(User))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] UserDTO user)
        {
            var rightUser = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
            };
            var result = await _accessControl.RegistrationAsync(rightUser);
            return Ok();
        }


        // PUT api/user/[userId]
        // BODY: (JSON)
        [HttpPut("{userId}")]
        [ProducesResponseType(201, Type = typeof(User))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid userId, [FromBody] UserDTO user)
        {
            var existUser = await _userManager.GetUserByIdAsync(userId);
            if (existUser == null)
            {
                return NotFound();
            }


            if (user.UserName != null) existUser.UserName = user.UserName;
            if (user.Email != null) existUser.Email = user.Email;
            _ = await _userManager.UpdateUserAsync(userId, existUser);
            return NoContent();
        }


        // POST api/user/[userId]/[duration]
        // Default 100 years
        [HttpPost("{userId:guid}/{duration}")]
        [ProducesResponseType(201, Type = typeof(User))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid userId, TimeSpan duration)
        {
            var existUser = await _userManager.GetUserByIdAsync(userId);
            if (existUser == null)
            {
                return NotFound();
            }
            var _ = await _userManager.BlockUserByIdAsync(userId, duration);
            return NoContent();
        }

    }
}

