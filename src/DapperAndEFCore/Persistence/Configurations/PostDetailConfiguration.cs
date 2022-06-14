using DapperAndEFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dapper.Persistence.Configurations;

public class PostDetailConfiguration : IEntityTypeConfiguration<PostDetail>
{
    public void Configure(EntityTypeBuilder<PostDetail> builder)
    {
        builder.Ignore(t => t.Id);

        builder.HasKey(t => t.PostId);
    }
}
