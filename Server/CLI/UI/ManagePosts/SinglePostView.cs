using Entities;
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
        
        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid post ID.");
            return;
        }
        
        Post post;
        try
        {
            post = await postRepository.GetSingleAsync(postId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return;
        }
        
        User author;
        try
        {
            author = await userRepository.GetSingleAsync(post.UserId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading author: {ex.Message}");
            return;
        }
        
        Console.WriteLine($"\n=== Post Details ===");
        Console.WriteLine($"ID: {post.Id}");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Author: {author.UserName}");
        Console.WriteLine($"Body: {post.Body}");
        
        await DisplayCommentsAsync(postId);
    }

    private async Task DisplayCommentsAsync(int postId)
    {
        var comments = commentRepository.GetMany().Where(c => c.PostId == postId).ToList();
        
        Console.WriteLine($"\nComments ({comments.Count}):");
        if (!comments.Any())
        {
            Console.WriteLine("No comments found.");
            return;
        }

        foreach (var comment in comments)
        {
            try
            {
                var commentAuthor = await userRepository.GetSingleAsync(comment.UserId);
                Console.WriteLine($"- [{comment.Id}] {commentAuthor.UserName}: {comment.Body}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"- [Error loading comment {comment.Id}: {ex.Message}]");
            }
        }
    }
}
