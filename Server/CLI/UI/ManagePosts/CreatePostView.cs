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
        Console.Write("Enter post body: ");
        string? body = Console.ReadLine();
        Console.Write("Enter user ID (author): ");

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Title and body cannot be empty!");
            return;
        }

        if (int.TryParse(Console.ReadLine(), out int userId))
        {
            try
            {
                // Verify user exists
                await userRepository.GetSingleAsync(userId);
                
                var post = new Post { Title = title, Body = body, UserId = userId };
                var createdPost = await postRepository.AddAsync(post);
                Console.WriteLine($"Post created successfully with ID: {createdPost.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating post: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Invalid user ID.");
        }
    }
}
