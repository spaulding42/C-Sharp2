#pragma warning disable CS8618
/* 
Disabled Warning: "Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable."
We can disable this safely because we know the framework will assign non-null values when it constructs this class for us.
*/
using Microsoft.EntityFrameworkCore;
namespace Vanlife.Models;
// the UsersContext class representing a session with our MySQL database, allowing us to query for or save data
public class VanlifeContext : DbContext 
{ 
    public VanlifeContext(DbContextOptions options) : base(options) { }
    // the "User" table name will come from the DbSet property name
    public DbSet<User> Users { get; set; } 

    public DbSet<Post> Posts { get; set; } 
    
    public DbSet<Comment> Comments { get; set; } 

    public DbSet<UserPostComment> UserPostComments { get; set; } 
}