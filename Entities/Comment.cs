namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }
    
    // Foreign keys
    public int UserId { get; set; }
    public int PostId { get; set; }
    
    // Navigation properties
    public User User { get; set; }
    public Post Post { get; set; }
    
    // Parameterless constructor for EFC
    private Comment() { }
    
    // Constructor for creating new comments
    public Comment(string body, int userId, int postId)
    {
        Body = body;
        UserId = userId;
        PostId = postId;
    }
}
