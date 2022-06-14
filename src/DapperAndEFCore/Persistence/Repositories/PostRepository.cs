using DapperAndEFCore.Entities;
using DapperAndEFCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace DapperAndEFCore.Persistence.Repositories;

public class PostRepository : BaseRepository<Post>, IPostRepository
{
    private readonly ApplicationContext _dbContext;
    private readonly IApplicationReadDbConnection _readDbConnection;
    private readonly IApplicationWriteDbConnection _writeDbConnection;

    public PostRepository(ApplicationContext dbContext, IApplicationReadDbConnection readDbConnection, IApplicationWriteDbConnection writeDbConnection) :
        base(dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _readDbConnection = readDbConnection ?? throw new ArgumentNullException(nameof(readDbConnection));
        _writeDbConnection = writeDbConnection ?? throw new ArgumentNullException(nameof(writeDbConnection));
    }

    // One-to-one
    public async Task<Post?> GetRelationOneToOneAsync(int id)
    {
        var result = await _readDbConnection.QueryMapAsync<Post, PostDetail, Post>(
            sql: "SELECT p.Id, p.UserId, p.Title, p.Body, pd.Created, pd.LastModified FROM Posts p INNER JOIN PostDetails pd ON p.Id = pd.PostId Where p.Id = @Id;",
            map: (post, detail) =>
            {
                post.Detail = detail;
                return post;
            },
            param: new { id },
            splitOn: "Created");

        return result.FirstOrDefault();
    }

    //One-to-many
    public async Task<Post?> GetRelationOneToManyAsync(int id)
    {
        var postMap = new Dictionary<int, Post>();

        var result = await _readDbConnection.QueryMapAsync<Post, Comment, Post>(
            sql: "SELECT p.Id, p.UserId, p.Title, p.Body, c.Id, c.PostId, c.Email, c.Name, c.Body FROM Posts p INNER JOIN Comments c ON p.Id = c.PostId Where p.Id = @Id;",
            map: (post, comment) =>
            {
                comment.PostId = post.Id; //non-reference back link

                //check if this order has been seen already
                if (postMap.TryGetValue(post.Id, out Post? existingPost))
                    post = existingPost;
                else
                    postMap.Add(post.Id, post);

                post.Comments.Add(comment);
                return post;
            },
            param: new { id },
            splitOn: "Id");

        return result.FirstOrDefault();
    }

    //Multi mapping
    public async Task<Post?> GetMultiMappingAsync(int id)
    {
        var postMap = new Dictionary<int, Post>();

        var result = await _readDbConnection.QueryMapAsync<Post, Comment, PostDetail, Post>(
            sql: "SELECT p.Id, p.UserId, p.Title, p.Body, " +
            "c.Id, c.PostId, c.Email, c.Name, c.Body, " +
            "pd.Created " +
            "FROM Posts p " +
            "INNER JOIN Comments c " +
            "ON p.Id = c.PostId " +
            "INNER JOIN PostDetails pd " +
            "ON p.Id = pd.PostId " +
            "Where p.Id = @Id;",
            map: (post, comment, detail) =>
            {
                if (post.Detail is null)
                    post.Detail = detail;

                comment.PostId = post.Id; //non-reference back link

                //check if this order has been seen already
                if (postMap.TryGetValue(post.Id, out Post? existingPost))
                    post = existingPost;
                else
                    postMap.Add(post.Id, post);

                post.Comments.Add(comment);
                return post;
            },
            param: new { id },
            splitOn: "Id,Created");

        return result.FirstOrDefault();
    }

    public async Task<IReadOnlyList<Post>> SearchPostByText(string text)
    {
        return await _readDbConnection.QueryAsync<Post>(
            sql: "SELECT * FROM Posts WHERE title LIKE @Text or body LIKE @Text",
            param: new { Text = $"%{ text.Trim() }%" });
    }

    /* Transaction Dapper and EF Core */
    public async Task SampleTransaction()
    {
        _dbContext.Connection.Open();

        using var transaction = _dbContext.Connection.BeginTransaction();

        try
        {
            // TRANSACTION
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

            // add user with EF Core
            var user = new User { Name = "Ervin Howell", Email = "Julianne.OConner@kory.org", Username = "Clementine", Address = new Address("Douglas Extension", "McKenziehaven", "McKenziehaven", "Germany", "59590-4157") };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // add post with Dapper
            var postId = await _writeDbConnection.QuerySingleAsync<int>(
                sql: $"insert into Posts(UserId, Title, Body) values (@User, @Title, @Body);SELECT CAST(SCOPE_IDENTITY() as int)",
                param: new { User = 1, Title = "ullam et saepe reiciendis voluptatem", Body = "nsit amet autem assumenda provident rerum culpa" },
                transaction: transaction
                );

            if (postId == 0) throw new Exception("error post id");

            // add detail with EF Core
            var detail = new PostDetail { PostId = postId, Created = DateTime.Now };
            await _dbContext.PostDetails.AddAsync(detail);
            await _dbContext.SaveChangesAsync();

            // add comments with Dapper
            var count = await _writeDbConnection.ExecuteAsync(
                sql: @"insert into Comments(PostId, Email, Name, Body) values (@PostId, @Email, @Name, @Body)",
                param: new Comment[] {
                        new Comment { PostId = postId, Email = "Shanna@melissa.tv", Name = "sunt aut facere repellat provident", Body = "occaecati excepturi optio reprehenderit" },
                        new Comment { PostId = postId, Email = "Clementine Bauch", Name = "ea molestias quasi exercitationem", Body = "doloribus vel accusantium quis pariatur" }
                },
                transaction: transaction
              );

            if (count != 2) throw new Exception("error adding posts");

            transaction.Commit();
            // COMMIT
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            _dbContext.Connection.Close();
        }
    }
}