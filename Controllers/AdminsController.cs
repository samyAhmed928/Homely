using Homely_modified_api.Data;
using Homely_modified_api.Dtos;
using Homely_modified_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homely_modified_api.Controllers
{
    [ApiController]
    [Route("api/admins")]
    public class AdminsController : ControllerBase
    {
        private readonly HomelyDbcontext _homelyDBContext;

        public AdminsController(HomelyDbcontext homelyDBContext)
        {
            _homelyDBContext = homelyDBContext;
        }
        [HttpGet("{id:guid}", Name = "GetAdminById")]
        public async Task<IActionResult> GetAdminById(Guid id)
        {
            var admin = await _homelyDBContext.Admins.FirstOrDefaultAsync(x => x.Id == id);
            if (admin == null)
            {
                return NotFound("Client not found.");
            }
            return Ok(admin);
        }
        [HttpPost("signup")]
        public async Task<IActionResult> Signup(Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_homelyDBContext.Admins.Any(c => c.Email == admin.Email))
            {
                return BadRequest("Email already exists.");
            }
            admin.Id = Guid.NewGuid();
            admin.SetPassword(admin.Password); // Assuming PasswordHash is set from request body

            _homelyDBContext.Admins.Add(admin);
            await _homelyDBContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdminById), new { id = admin.Id }, admin);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Email and password are required");
            }

            var admin = await _homelyDBContext.Admins.FirstOrDefaultAsync(a => a.Email == loginDto.Email);
            if (admin == null || !admin.ValidatePassword(loginDto.Password))
            {
                return Unauthorized("Invalid credentials");
            }

            // Generate a JWT or implement other authentication mechanism
            return Ok(new { message = "Login successful" }); // Replace with actual token
        }


    }

}

