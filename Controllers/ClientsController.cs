using Homely_modified_api.Data;
using Homely_modified_api.Dtos;
using Homely_modified_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homely_modified_api.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly HomelyDbcontext _homelyDBContext;

        public ClientsController(HomelyDbcontext homelyDBContext)
        {
            _homelyDBContext = homelyDBContext;
        }
        [HttpGet("{id:guid}", Name = "GetClientById")]
        public async Task<IActionResult> GetClientById(Guid id)
        {
            var client = await _homelyDBContext.Clients.FirstOrDefaultAsync(x => x.Id == id);
            if (client == null)
            {
                return NotFound("Client not found.");
            }
            return Ok(client);
        }
        [HttpPost("signup")]
        public async Task<IActionResult> Signup(ClientDTO clientDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_homelyDBContext.Clients.Any(c => c.Email == clientDTO.Email))
            {
                return BadRequest("Email already exists.");
            }
            var client = new Client
            {
                Id = Guid.NewGuid(),
                Name = clientDTO.Name,
                Phone = clientDTO.Phone,
                Email = clientDTO.Email,
                
                Plan = 0,
                NumberOfAdds=1
            };
            client.SetPassword(clientDTO.Password);
             // Setting NumberOfAdds based on the plan

            _homelyDBContext.Clients.Add(client);
            await _homelyDBContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, client);
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
            var client = await _homelyDBContext.Clients.FirstOrDefaultAsync(a => a.Email == loginDto.Email);
            if (client == null || !client.ValidatePassword(loginDto.Password))
            {
                return Unauthorized("Invalid credentials");
            }

            // Generate a JWT or implement other authentication mechanism
            return Ok(client); // Replace with actual token
        }

        [HttpPost("subscribe/{id:guid}")]
        public async Task<IActionResult> Subscribe(Guid id, [FromBody] int plan)
        {
            var client = await _homelyDBContext.Clients.FirstOrDefaultAsync(x => x.Id == id);

            if (client == null)
            {
                return NotFound("Client not found.");
            }
            client.Plan=plan;
            // Set NumberOfAdds based on the plan
            switch (plan)
            {
                case 0:
                    client.NumberOfAdds = 1;
                    break;
                case 1:
                    client.NumberOfAdds = 5;
                    break;
                case 2:
                    client.NumberOfAdds = 10;
                    break;
                case 3:
                    client.NumberOfAdds = 20;
                    break;
                case 4:
                    client.NumberOfAdds = 50;
                    break;
                default:
                    return BadRequest("Invalid plan type");
            }

            await _homelyDBContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, client);
        }




    }
}

