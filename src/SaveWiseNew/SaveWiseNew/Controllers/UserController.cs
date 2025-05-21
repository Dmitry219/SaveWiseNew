using Microsoft.AspNetCore.Mvc;
using SaveWise.Model;
using SaveWise.Repositories;
using SaveWiseNew.Service;
using System.Threading.Tasks;

namespace SaveWiseNew.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            if(user == null)
            {
                return BadRequest();
            }

            var createUser = await _userService.Add(user);
            if (createUser == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetAll), new { id = createUser.Id }, createUser);
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _userService.Get();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _userService.Get(id);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _userService.Delete(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
