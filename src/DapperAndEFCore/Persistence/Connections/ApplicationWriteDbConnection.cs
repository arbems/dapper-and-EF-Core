using Dapper;
using DapperAndEFCore.Interfaces;
using System.Data;

namespace DapperAndEFCore.Persistence.Connections;

public class ApplicationWriteDbConnection : IApplicationWriteDbConnection
{
    private readonly IApplicationContext context;

    public ApplicationWriteDbConnection(IApplicationContext context)
    {
        this.context = context;
    }

    public async Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        return await context.Connection.ExecuteAsync(sql, param, transaction);
    }

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        return (await context.Connection.QueryAsync<T>(sql, param, transaction)).AsList();
    }

    public async Task<IEnumerable<TResult>> QueryMapAsync<T1, T2, TResult>(string sql, Func<T1, T2, TResult> map, object? param = null, IDbTransaction? transaction = null, string splitOn = "Id", CancellationToken cancellationToken = default)
    {
        return await context.Connection.QueryAsync(sql, map, param, transaction, true, splitOn);
    }

    public async Task<IEnumerable<TResult>> QueryMapAsync<T1, T2, T3, TResult>(string sql, Func<T1, T2, T3, TResult> map, object? param = null, IDbTransaction? transaction = null, string splitOn = "Id", CancellationToken cancellationToken = default)
    {
        return await context.Connection.QueryAsync(sql, map, param, transaction, true, splitOn);
    }

    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        return await context.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
    }

    public async Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        return await context.Connection.QuerySingleAsync<T>(sql, param, transaction);
    }
}