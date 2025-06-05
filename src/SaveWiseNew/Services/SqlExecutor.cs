using Dapper;
using SaveWiseNew.Services.Interfaces;
using System.Data;

namespace SaveWiseNew.Services;

public class SqlExecutor : ISqlExecutor
{
    private readonly IDbConnection _connection;

    public SqlExecutor(IDbConnection connection)
    {
        _connection = connection;
    }

    public Task<int> ExecuteAsync(string sql, object? param = null)
    {
        return _connection.ExecuteAsync(sql, param);
    }

    public Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
    {
        return _connection.QueryAsync<T>(sql, param);
    }

    public Task<T> QuerySingleAsync<T>(string sql, object? param = null)
    {
        return _connection.QuerySingleAsync<T>(sql, param);
    }

    public Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
    {
        return _connection.QueryFirstOrDefaultAsync<T>(sql, param);
    }
}
