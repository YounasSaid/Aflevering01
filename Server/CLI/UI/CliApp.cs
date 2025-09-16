using CLI.UI.ManageComments;
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

            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Please try again.");
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
            try
            {
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
                        var manageUsersView = new SingleUserView(userRepository);
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Please try again.");
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
            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Please try again.");
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
            try
            {
                switch (choice)
                {
                    case "1":
                        var addCommentView = new AddCommentView(commentRepository, postRepository, userRepository);
                        await addCommentView.AddCommentToPostAsync();
                        break;
                    case "2":
                        var listCommentsView = new ListCommentsView(commentRepository, postRepository, userRepository);
                        await listCommentsView.ViewAllCommentsAsync();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Please try again.");
            }
        }
    }
}
