using DapperAndEFCore.Entities;

namespace DapperAndEFCore.Persistence;

public static class ApplicationContextSeed
{
    public static async Task SeedSampleDataAsync(ApplicationContext context)
    {
        // Seed, if necessary
        if (!context.Users.Any())
        {
            var user = new User
            {
                Name = "Alberto",
                Email = "contacto@arbems.com",
                Username = "arbems",
                Address = new Address("Calle sierpes", "Sevilla", "Sevilla", "España", "41005"),
                Posts = { }
            };

            context.Users.Add(user);

            await context.SaveChangesAsync();

            var post1 = new Post
            {
                Title = "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
                Body = "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto",
                UserId = user.Id,
                Comments = { }
            };

            context.Posts.Add(post1);

            await context.SaveChangesAsync();

            var comment1 = new Comment { Name = "Oscar", Body = "id labore ex et quam laborum", Email = "Lew@alysha.tv", PostId = post1.Id };
            var comment2 = new Comment { Name = "Jesus", Body = "odio adipisci rerum aut animi", Email = "Hayden@althea.biz", PostId = post1.Id };
            var comment3 = new Comment { Name = "Marta", Body = "vero eaque aliquid doloribus et culpa", Email = "Presley.Mueller@myrl.com", PostId = post1.Id };

            context.Comments.Add(comment1);
            context.Comments.Add(comment2);
            context.Comments.Add(comment3);

            var post2 = new Post
            {
                Title = "qui est esse",
                Body = "st rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus ",
                UserId = user.Id,
                Comments = { }
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
                Comments = { }
            };

            context.Posts.Add(post3);

            await context.SaveChangesAsync();

            var comment5 = new Comment { Name = "Elena", Body = "et omnis dolorem", Email = "Mallory_Kunze@marie.org", PostId = post3.Id };
            var comment6 = new Comment { Name = "Carlos", Body = "provident id voluptas", Email = "Meghan_Littel@rene.us", PostId = post3.Id };

            context.Comments.Add(comment5);
            context.Comments.Add(comment6);

            await context.SaveChangesAsync();
        }
    }
}
