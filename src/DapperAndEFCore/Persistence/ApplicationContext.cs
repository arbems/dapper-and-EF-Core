using DapperAndEFCore.Entities;
using DapperAndEFCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace DapperAndEFCore.Persistence;

public class ApplicationContext : DbContext, IApplicationContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<PostDetail> PostDetails => Set<PostDetail>();

    // Representa una conexión abierta a un origen de datos y
    // lo implementan proveedores de datos .NET que acceden a bases de datos relacionales.
    public IDbConnection Connection => Database.GetDbConnection();

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}



/* 
 * Sample Data 
 */
public static class ApplicationContextSeed
{
    public static async Task SeedSampleDataAsync(ApplicationContext context)
    {
        // Seed, if necessary
        if (!context.Users.Any())
        {
            var user = new User
            {
                Name = "Leanne Graham",
                Email = "Sincere@april.biz",
                Username = "Bret",
                Address = new Address("Kulas Light", "Gwenborough", "Gwenborough", "Germany", "92998-3874"),
                Posts = { }
            };

            context.Users.Add(user);

            await context.SaveChangesAsync();

            var post1 = new Post
            {
                Title = "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
                Body = "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto",
                UserId = user.Id,
                Comments = { },
                Detail = new PostDetail { Created = DateTime.Now }
            };

            context.Posts.Add(post1);

            await context.SaveChangesAsync();

            var comment1 = new Comment { Name = "Ervin Howell", Body = "id labore ex et quam laborum", Email = "Lew@alysha.tv", PostId = post1.Id };
            var comment2 = new Comment { Name = "Clementine Bauch", Body = "odio adipisci rerum aut animi", Email = "Hayden@althea.biz", PostId = post1.Id };
            var comment3 = new Comment { Name = "Romaguera-Jacobson", Body = "vero eaque aliquid doloribus et culpa", Email = "Presley.Mueller@myrl.com", PostId = post1.Id };

            context.Comments.Add(comment1);
            context.Comments.Add(comment2);
            context.Comments.Add(comment3);

            var post2 = new Post
            {
                Title = "qui est esse",
                Body = "st rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus ",
                UserId = user.Id,
                Comments = { },
                Detail = new PostDetail { Created = DateTime.Now }
            };

            context.Posts.Add(post2);

            await context.SaveChangesAsync();

            var comment4 = new Comment { Name = "Ana", Body = "et fugit eligendi deleniti quidem qui sint nihil autem", Email = "Dallas@ole.me", PostId = post2.Id };


            context.Comments.Add(comment4);

            var post3 = new Post
            {
                Title = "ea molestias quasi exercitationem repellat qui ipsa sit aut",
                Body = "et iusto sed quo iure\nvoluptatem occaecati omnis eligendi aut ad\nvoluptatem doloribus vel accusantium quis pariatur\nmolestiae porro eius odio et labore et velit aut",
                UserId = user.Id,
                Comments = { },
                Detail = new PostDetail { Created = DateTime.Now }
            };

            context.Posts.Add(post3);

            await context.SaveChangesAsync();

            var comment5 = new Comment { Name = "Chelsey Dietrich", Body = "et omnis dolorem", Email = "Mallory_Kunze@marie.org", PostId = post3.Id };
            var comment6 = new Comment { Name = "Dennis Schulist", Body = "provident id voluptas", Email = "Meghan_Littel@rene.us", PostId = post3.Id };

            context.Comments.Add(comment5);
            context.Comments.Add(comment6);

            await context.SaveChangesAsync();
        }
    }
}