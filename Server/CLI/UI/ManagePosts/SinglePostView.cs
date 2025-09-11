using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;

    public SinglePostView(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ViewSpecificPostAsync()
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
}
