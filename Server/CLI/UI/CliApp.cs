using RepositoryContracts;

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
                    await CreateUserAsync();
                    break;
                case "2":
                    await ViewAllUsersAsync();
                    break;
                case "3":
                    await ViewSpecificUserAsync();
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
                    await CreatePostAsync();
                    break;
                case "2":
                    await ViewPostsOverviewAsync();
                    break;
                case "3":
                    await ViewSpecificPostAsync();
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

    // User methods
    private async Task CreateUserAsync()
    {
        Console.WriteLine("\n--- Create New User ---");
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();
        Console.Write("Enter password: ");
        string? password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Username and password cannot be empty!");
            return;
        }

        try
        {
            var user = new Entities.User { UserName = username, Password = password };
            var createdUser = await userRepository.AddAsync(user);
            Console.WriteLine($"User created successfully with ID: {createdUser.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating user: {ex.Message}");
        }
    }

    private async Task ViewAllUsersAsync()
    {
        Console.WriteLine("\n--- All Users ---");
        var users = userRepository.GetMany().ToList();
        
        if (!users.Any())
        {
            Console.WriteLine("No users found.");
            return;
        }

        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id}, Username: {user.UserName}");
        }
    }

    private async Task ViewSpecificUserAsync()
    {
        Console.Write("Enter user ID: ");
        if (int.TryParse(Console.ReadLine(), out int userId))
        {
            try
            {
                var user = await userRepository.GetSingleAsync(userId);
                Console.WriteLine($"\nUser Details:");
                Console.WriteLine($"ID: {user.Id}");
                Console.WriteLine($"Username: {user.UserName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Invalid user ID.");
        }
    }

    // Post methods
    private async Task CreatePostAsync()
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
                
                var post = new Entities.Post { Title = title, Body = body, UserId = userId };
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

    private async Task ViewPostsOverviewAsync()
    {
        Console.WriteLine("\n--- Posts Overview ---");
        var posts = postRepository.GetMany().ToList();
        
        if (!posts.Any())
        {
            Console.WriteLine("No posts found.");
            return;
        }

        foreach (var post in posts)
        {
            Console.WriteLine($"[{post.Id}] {post.Title}");
        }
    }

    private async Task ViewSpecificPostAsync()
    {
        Console.Write("Enter post ID: ");
        if (int.TryParse(Console.ReadLine(), out int postId))
        {
            try
            {
                var post = await postRepository.GetSingleAsync(postId);
                var author = await userRepository.GetSingleAsync(post.UserId);
                var comments = commentRepository.GetMany().Where(c => c.PostId == postId).ToList();

                Console.WriteLine($"\n=== Post Details ===");
                Console.WriteLine($"Title: {post.Title}");
                Console.WriteLine($"Author: {author.UserName}");
                Console.WriteLine($"Body: {post.Body}");
                Console.WriteLine($"\nComments ({comments.Count}):");
                
                foreach (var comment in comments)
                {
                    var commentAuthor = await userRepository.GetSingleAsync(comment.UserId);
                    Console.WriteLine($"- {commentAuthor.UserName}: {comment.Body}");
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

    // Comment methods
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