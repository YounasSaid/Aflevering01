using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task CreatePostAsync()
    {
        Console.WriteLine("\n--- Create New Post ---");

        Console.Write("Enter post title: ");
        string? title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Title cannot be empty!");
            return;
        }

        Console.Write("Enter post body: ");
        string? body = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Body cannot be empty!");
            return;
        }
        
        Console.Write("Enter user ID (author): ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID.");
            return;
        }
        
        try
        {
            await userRepository.GetSingleAsync(userId);
        }
        catch (Exception)
        {
            Console.WriteLine($"User with ID {userId} was not found.");
            return;
        }
        
        var post = new Post 
        { 
            Title = title.Trim(), 
            Body = body.Trim(), 
            UserId = userId 
        };
    
        try
        {
            var createdPost = await postRepository.AddAsync(post);
            Console.WriteLine($"Post created successfully with ID: {createdPost.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create post: {ex.Message}");
        }
    }
}
