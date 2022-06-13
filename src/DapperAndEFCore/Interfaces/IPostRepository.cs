using DapperAndEFCore.Entities;

namespace DapperAndEFCore.Interfaces;

public interface IPostRepository : IRepository<Post>, IReadRepository<Post>
{
    Task<IEnumerable<Post>> SearchPost(string text);
}