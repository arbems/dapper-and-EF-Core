using DapperAndEFCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DapperAndEFCore.Interfaces;

public interface IApplicationContext : IUnitOfWork
{
    DbSet<User> Users { get; }
    DbSet<Post> Posts { get; }
    DbSet<Comment> Comments { get; }

    public IDbConnection Connection { get; }
}