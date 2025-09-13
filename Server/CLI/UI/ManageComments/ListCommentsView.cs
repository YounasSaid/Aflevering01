using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView
{
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    
    public ListCommentsView(ICommentRepository commentRepository, IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }
    public async Task ViewAllCommentsAsync()
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