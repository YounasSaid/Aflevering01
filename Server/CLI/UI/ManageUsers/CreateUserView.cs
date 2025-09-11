using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task CreateUserAsync()
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
            var user = new User(username, password); // Fixed constructor call
            var createdUser = await userRepository.AddAsync(user);
            Console.WriteLine($"User created successfully with ID: {createdUser.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating user: {ex.Message}");
        }
    }
}
