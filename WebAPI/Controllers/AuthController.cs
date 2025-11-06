using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequest request)
    {
        // Find user by username
        var user = userRepository.GetMany()
            .FirstOrDefault(u => u.UserName.ToLower() == request.UserName.ToLower());

        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }

        // Check password
        if (user.Password != request.Password)
        {
            return Unauthorized("Invalid username or password");
        }

        // Return user info (without password!)
        var userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName
        };

        return Ok(userDto);
    }
}
