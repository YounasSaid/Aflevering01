using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class SingleUserView
{
    private readonly IUserRepository userRepository;

    public SingleUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task ViewSpecificUserAsync()
    {
        Console.Write("Enter user ID: ");
        
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID.");
            return;
        }
        
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
}
