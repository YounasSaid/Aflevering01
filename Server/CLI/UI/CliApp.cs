using RepositoryContracts;
using CLI.UI.ManageUsers;
using CLI.UI.ManagePosts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public CliApp(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.userRepository = userRepository;
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task StartAsync()
    {
        Console.WriteLine("Welcome to the Forum CLI!");
        Console.WriteLine("========================");

        bool running = true;
        while (running)
        {
            ShowMainMenu();
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ManageUsersAsync();
                    break;
                case "2":
                    await ManagePostsAsync();
                    break;
                case "3":
                    await ManageCommentsAsync();
                    break;
                case "0":
                    running = false;
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            
            if (running)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    private void ShowMainMenu()
    {
        Console.WriteLine("\n=== MAIN MENU ===");
        Console.WriteLine("1. Manage Users");
        Console.WriteLine("2. Manage Posts");
        Console.WriteLine("3. Manage Comments");
        Console.WriteLine("0. Exit");
        Console.Write("Choose an option: ");
    }

    private async Task ManageUsersAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n=== USER MANAGEMENT ===");
            Console.WriteLine("1. Create new user");
            Console.WriteLine("2. View all users");
            Console.WriteLine("3. View specific user");
            Console.WriteLine("0. Back to main menu");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    var createUserView = new CreateUserView(userRepository);
                    await createUserView.CreateUserAsync();
                    break;
                case "2":
                    var listUsersView = new ListUsersView(userRepository);
                    await listUsersView.ViewAllUsersAsync();
                    break;
                case "3":
                    var manageUsersView = new ManageUsersView(userRepository);
                    await manageUsersView.ViewSpecificUserAsync();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private async Task ManagePostsAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n=== POST MANAGEMENT ===");
            Console.WriteLine("1. Create new post");
            Console.WriteLine("2. View posts overview");
            Console.WriteLine("3. View specific post");
            Console.WriteLine("0. Back to main menu");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    var createPostView = new CreatePostView(postRepository, userRepository);
                    await createPostView.CreatePostAsync();
                    break;
                case "2":
                    var listPostsView = new ListPostsView(postRepository);
                    await listPostsView.ViewPostsOverviewAsync();
                    break;
                case "3":
                    var singlePostView = new SinglePostView(postRepository, userRepository, commentRepository);
                    await singlePostView.ViewSpecificPostAsync();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private async Task ManageCommentsAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n=== COMMENT MANAGEMENT ===");
            Console.WriteLine("1. Add comment to post");
            Console.WriteLine("2. View all comments");
            Console.WriteLine("0. Back to main menu");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await AddCommentToPostAsync();
                    break;
                case "2":
                    await ViewAllCommentsAsync();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    // Comment methods (keeping these here for now)
    private async Task AddCommentToPostAsync()
    {
        Console.WriteLine("\n--- Add Comment to Post ---");
        Console.Write("Enter post ID: ");
        if (int.TryParse(Console.ReadLine(), out int postId))
        {
            try
            {
                // Verify post exists
                await postRepository.GetSingleAsync(postId);
                
                Console.Write("Enter your user ID: ");
                if (int.TryParse(Console.ReadLine(), out int userId))
                {
                    // Verify user exists
                    await userRepository.GetSingleAsync(userId);
                    
                    Console.Write("Enter comment: ");
                    string? body = Console.ReadLine();
                    
                    if (string.IsNullOrWhiteSpace(body))
                    {
                        Console.WriteLine("Comment cannot be empty!");
                        return;
                    }

                    var comment = new Entities.Comment { Body = body, UserId = userId, PostId = postId };
                    var createdComment = await commentRepository.AddAsync(comment);
                    Console.WriteLine($"Comment added successfully with ID: {createdComment.Id}");
                }
                else
                {
                    Console.WriteLine("Invalid user ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Invalid post ID.");
        }
    }

    private async Task ViewAllCommentsAsync()
    {
        Console.WriteLine("\n--- All Comments ---");
        var comments = commentRepository.GetMany().ToList();
        
        if (!comments.Any())
        {
            Console.WriteLine("No comments found.");
            return;
        }

        foreach (var comment in comments)
        {
            try
            {
                var author = await userRepository.GetSingleAsync(comment.UserId);
                var post = await postRepository.GetSingleAsync(comment.PostId);
                Console.WriteLine($"[{comment.Id}] {author.UserName} on '{post.Title}': {comment.Body}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Comment ID {comment.Id}: Error loading details - {ex.Message}");
            }
        }
    }
}
