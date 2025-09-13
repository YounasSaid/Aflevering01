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
        
        // Guard Clause 1: Get and validate username
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Username cannot be empty!");
            return;
        }

        // Guard Clause 2: Get and validate password
        Console.Write("Enter password: ");
        string? password = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Password cannot be empty!");
            return;
        }

        // Guard Clause 3: Check if username is already taken
        var existingUsers = userRepository.GetMany().Where(u => 
            u.UserName.ToLower() == username.Trim().ToLower()).ToList();
        if (existingUsers.Any())
        {
            Console.WriteLine($"Username '{username.Trim()}' is already taken!");
            return;
        }

        // Happy Path: Create the user
        try
        {
            var user = new User(username.Trim(), password.Trim());
            var createdUser = await userRepository.AddAsync(user);
            Console.WriteLine($"User created successfully with ID: {createdUser.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create user: {ex.Message}");
        }
    }
}
