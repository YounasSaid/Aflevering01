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
        
        if (int.TryParse(Console.ReadLine(),out int postId))
        {
            try
            {
                var post = await postRepository.GetSingleAsync(postId);
                var author = await userRepository.GetSingleAsync(post.UserId);
                
                Console.WriteLine($"\n post Details:");
                Console.WriteLine($"Author: {author.UserName}");
                Console.WriteLine($"ID: {post.Id}");
                Console.WriteLine($"Title: {post.Title}");
                Console.WriteLine($"Body: {post.Body}");
                
                

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}"); 
            }
        }
        else
        {
            Console.WriteLine($"Invalid post ID");
            
        }
        
    }
}
