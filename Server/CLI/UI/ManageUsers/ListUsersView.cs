using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task ViewAllUsersAsync()
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
}
