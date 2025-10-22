using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using WebAPI.Exceptions;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        await VerifyUserNameIsAvailableAsync(request.UserName);

        User user = new(request.UserName, request.Password);
        User created = await userRepo.AddAsync(user);
        UserDto dto = new()
        {
            Id = created.Id,
            UserName = created.UserName
        };
        return Created($"/Users/{dto.Id}", dto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto request)
    {
        User? existing = await userRepo.GetSingleAsync(id);
        if (existing == null)
        {
            return NotFound($"User with id {id} not found");
        }

        User updatedUser = new(request.UserName, request.Password)
        {
            Id = id
        };

        await userRepo.UpdateAsync(updatedUser);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser([FromRoute] int id)
    {
        try
        {
            await userRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingleUser([FromRoute] int id)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
            UserDto dto = new()
            {
                Id = user.Id,
                UserName = user.UserName
            };
            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers([FromQuery] string? userNameContains = null)
    {
        IQueryable<User> query = userRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(userNameContains))
        {
            query = query.Where(u => u.UserName.ToLower().Contains(userNameContains.ToLower()));
        }

        IList<UserDto> users = query.Select(u => new UserDto
        {
            Id = u.Id,
            UserName = u.UserName
        }).ToList();

        return Ok(users);
    }

    private async Task VerifyUserNameIsAvailableAsync(string userName)
    {
        bool userNameTaken = userRepo.GetMany().Any(u => u.UserName.ToLower() == userName.ToLower());
        if (userNameTaken)
        {
            throw new InvalidOperationException($"Username '{userName}' is already taken");
        }
    }
}
