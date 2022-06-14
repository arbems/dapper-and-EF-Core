using DapperAndEFCore.Entities;

namespace DapperAndEFCore.Interfaces;

public interface IPostRepository : IRepository<Post>, IReadRepository<Post>
{
    Task<Post?> GetRelationOneToOneAsync(int id);

    Task<Post?> GetRelationOneToManyAsync(int id);

    Task<Post?> GetMultiMappingAsync(int id);

    Task<IReadOnlyList<Post>> SearchPostByText(string text);

    Task SampleTransaction();
}