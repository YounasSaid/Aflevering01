using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    private readonly ICommentRepository commentRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo, ICommentRepository commentRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
        this.commentRepo = commentRepo;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
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

        // Brug den nye constructor
        Post post = new(request.Title, request.Body, request.UserId);

        Post created = await postRepo.AddAsync(post);

        PostDto dto = new()
        {
            Id = created.Id,
            Title = created.Title,
            Body = created.Body,
            UserId = created.UserId
        };

        return Created($"/Posts/{dto.Id}", dto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePost([FromRoute] int id, [FromBody] UpdatePostDto request)
    {
        try
        {
            Post existing = await postRepo.GetSingleAsync(id);

            existing.Title = request.Title;
            existing.Body = request.Body;

            await postRepo.UpdateAsync(existing);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePost([FromRoute] int id)
    {
        try
        {
            await postRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetPost(
        [FromRoute] int id,
        [FromQuery] bool includeAuthor = false,
        [FromQuery] bool includeComments = false)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);

            PostDto dto = new()
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
            };

            if (includeAuthor)
            {
                try
                {
                    User author = await userRepo.GetSingleAsync(post.UserId);
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

            if (includeComments)
            {
                IList<Comment> comments = commentRepo.GetMany()
                    .Where(c => c.PostId == post.Id)
                    .ToList();

                dto.Comments = comments.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Body = c.Body,
                    UserId = c.UserId,
                    PostId = c.PostId
                }).ToList();
            }

            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetPosts(
        [FromQuery] string? titleContains = null,
        [FromQuery] int? userId = null,
        [FromQuery] string? userName = null)
    {
        IQueryable<Post> query = postRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(titleContains))
        {
            query = query.Where(p => p.Title.ToLower().Contains(titleContains.ToLower()));
        }

        if (userId.HasValue)
        {
            query = query.Where(p => p.UserId == userId.Value);
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            // Get users matching the name
            var userIds = userRepo.GetMany()
                .Where(u => u.UserName.ToLower().Contains(userName.ToLower()))
                .Select(u => u.Id)
                .ToList();

            query = query.Where(p => userIds.Contains(p.UserId));
        }

        IList<PostDto> posts = query.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            UserId = p.UserId
        }).ToList();

        return Ok(posts);
    }
}
