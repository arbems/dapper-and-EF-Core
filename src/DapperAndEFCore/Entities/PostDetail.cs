using DapperAndEFCore.Entities.Common;

namespace DapperAndEFCore.Entities;

public class PostDetail : Entity
{
    public Post Post { get; set; }
    public int PostId { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? LastModified { get; set; }
}
