using InMemoryRepositories;
using RepositoryContracts;
using CLI.UI;

// Instantiate repositories - ONLY place where concrete implementations are created
IUserRepository userRepository = new UserInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();

// Create and start CLI app
CliApp app = new CliApp(userRepository, postRepository, commentRepository);
await app.StartAsync();