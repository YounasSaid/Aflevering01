using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IUserRepository userRepo;
    private readonly IPostRepository postRepo;

    public CommentsController(ICommentRepository commentRepo, IUserRepository userRepo, IPostRepository postRepo)
    {
        this.commentRepo = commentRepo;
        this.userRepo = userRepo;
        this.postRepo = postRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
    {
        // Verify user exists
        try
        {
            await userRepo.GetSingleAsync(request.UserId);
        }
        catch (InvalidOperationException)
        {
            return BadRequest($"User with id {request.UserId} not found");
        }

        // Verify post exists
        try
        {
            await postRepo.GetSingleAsync(request.PostId);
        }
        catch (InvalidOperationException)
        {
            return BadRequest($"Post with id {request.PostId} not found");
        }

        Comment comment = new(request.Body, request.UserId, request.PostId);

        Comment created = await commentRepo.AddAsync(comment);

        CommentDto dto = new()
        {
            Id = created.Id,
            Body = created.Body,
            UserId = created.UserId,
            PostId = created.PostId
        };

        return Created($"/Comments/{dto.Id}", dto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDto request)
    {
        try
        {
            Comment existing = await commentRepo.GetSingleAsync(id);

            existing.Body = request.Body;

            await commentRepo.UpdateAsync(existing);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment([FromRoute] int id)
    {
        try
        {
            await commentRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetComment(
        [FromRoute] int id,
        [FromQuery] bool includeAuthor = false)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);

            CommentDto dto = new()
            {
                Id = comment.Id,
                Body = comment.Body,
                UserId = comment.UserId,
                PostId = comment.PostId
            };

            if (includeAuthor)
            {
                try
                {
                    User author = await userRepo.GetSingleAsync(comment.UserId);
                    dto.Author = new UserDto
                    {
                        Id = author.Id,
                        UserName = author.UserName
                    };
                }
                catch
                {
                    // Author not found, leave as null
                }
            }

            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetComments(
        [FromQuery] int? userId = null,
        [FromQuery] string? userName = null,
        [FromQuery] int? postId = null)
    {
        IQueryable<Comment> query = commentRepo.GetMany();

        if (userId.HasValue)
        {
            query = query.Where(c => c.UserId == userId.Value);
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            // Get users matching the name
            var userIds = userRepo.GetMany()
                .Where(u => u.UserName.ToLower().Contains(userName.ToLower()))
                .Select(u => u.Id)
                .ToList();

            query = query.Where(c => userIds.Contains(c.UserId));
        }

        if (postId.HasValue)
        {
            query = query.Where(c => c.PostId == postId.Value);
        }

        IList<CommentDto> comments = query.Select(c => new CommentDto
        {
            Id = c.Id,
            Body = c.Body,
            UserId = c.UserId,
            PostId = c.PostId
        }).ToList();

        return Ok(comments);
    }
}
