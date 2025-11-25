using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class ForumDbContext : DbContext
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=C:\Users\Youna\OneDrive - ViaUC\3 SEM\DNP 1\Aflevering01\Server\EfcRepositories\app.db");
    }
}
