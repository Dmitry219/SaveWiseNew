using Microsoft.AspNetCore.Mvc;
using SaveWiseNew.ApiContracts;
using SaveWiseNew.DataAccess.Models;
using SaveWiseNew.Services.Interfaces;

namespace SaveWiseNew.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost]
    public async Task<ActionResult<User>> Post([FromBody] User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createUser = await _userService.Add(user);
        return CreatedAtAction(nameof(Get), new { id = createUser.Id }, createUser);
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> Get()
    {
        var users = await _userService.Get();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get([FromRoute] UserIdRequest request)
    {
        var user = await _userService.Get(request.Id);

        if (user == null)
            return NotFound($"User with Id={request.Id} not found");

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] UserIdRequest request)
    {
        var deleted = await _userService.Delete(request.Id);
        return deleted ? NoContent() : NotFound();
    }
}
