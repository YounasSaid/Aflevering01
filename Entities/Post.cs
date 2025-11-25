namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    
    // Foreign key
    public int UserId { get; set; }
    
    // Navigation properties
    public User User { get; set; }
    public List<Comment> Comments { get; set; } = new();
    
    // Parameterless constructor for EFC
    private Post() { }
    
    // Constructor for creating new posts
    public Post(string title, string body, int userId)
    {
        Title = title;
        Body = body;
        UserId = userId;
    }
}
