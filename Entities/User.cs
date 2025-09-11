namespace Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    
    // Parameterless constructor for EFC (kommer senere)
    private User() { }
    
    // Constructor for creating new users
    public User(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}