using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;

    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task ViewPostsOverviewAsync()
    {
        Console.WriteLine("\n--- Posts Overview ---");
        var posts = postRepository.GetMany().ToList();
        
        if (!posts.Any())
        {
            Console.WriteLine("No posts found.");
            return;
        }

        foreach (var post in posts)
        {
            Console.WriteLine($"[{post.Id}] {post.Title}");
        }
    }
}
