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

    public async Task<IEnumerable<Post>> SearchPost(string text)
    {
        var parameters = new { Text = $"%{ text.Trim() }%" };

        var query = "SELECT * FROM Posts WHERE title LIKE @Text or body LIKE @Text";
        var companies = await _readDbConnection.QueryAsync<Post>(query, parameters);
        return companies.ToList();
    }

    public async Task AddNewPostWithComment(Post post)
    {
        _dbContext.Connection.Open();
        using (var transaction = _dbContext.Connection.BeginTransaction())
        {
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                var addPostQuery = $"INSERT INTO Posts(UserId, Title, Body) VALUES({post.UserId}, '{post.Title}','{post.Body}');SELECT CAST(SCOPE_IDENTITY() as int)";
                var postId = await _writeDbConnection.QuerySingleAsync<int>(addPostQuery, transaction: transaction);

                if (postId == 0)
                {
                    throw new Exception("Post Id");
                }

                var comment = new Comment
                {
                    Body = post.Comments.First().Body,
                    Name = post.Comments.First().Name,
                    Email = post.Comments.First().Email,
                    PostId = postId,
                };
                await _dbContext.Comments.AddAsync(comment);
                await _dbContext.SaveChangesAsync(default);

                transaction.Commit();
            }
            catch (Exception)
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
}