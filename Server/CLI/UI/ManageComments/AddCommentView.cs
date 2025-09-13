using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class AddCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public AddCommentView(ICommentRepository commentRepository, IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task AddCommentToPostAsync()
    {
        Console.WriteLine("\n--- Add Comment to Post ---");

        Console.Write("Enter post ID: ");
        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid post ID.");
            return;
        }
        
        var post = await postRepository.GetSingleAsync(postId);
        if (post is null)
        {
            Console.WriteLine($"Post with ID {postId} was not found.");
            return;
        }

        Console.Write("Enter your user ID: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID.");
            return;
        }
        
        var user = await userRepository.GetSingleAsync(userId);
        if (user is null)
        {
            Console.WriteLine($"User with ID {userId} was not found.");
            return;
        }

        Console.Write("Enter comment: ");
        string? body = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Comment cannot be empty!");
            return;
        }

        var comment = new Entities.Comment
        {
            Body = body.Trim(),
            UserId = userId,
            PostId = postId
        };

        try
        {
            var created = await commentRepository.AddAsync(comment);
            Console.WriteLine($"Comment added successfully with ID: {created.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add comment: {ex.Message}");
        }
    }

    
}