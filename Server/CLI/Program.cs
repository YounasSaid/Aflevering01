using FileRepositories;
using RepositoryContracts;
using CLI.UI;

// Instantiate repositories - ONLY place where concrete implementations are created
IUserRepository userRepository = new UserFileRepository();
IPostRepository postRepository = new PostFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();

// Create and start CLI app
CliApp app = new CliApp(userRepository, postRepository, commentRepository);
await app.StartAsync();
