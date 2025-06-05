namespace SaveWiseNew.Services.Interfaces;

public interface ISqlExecutor
{
    Task<T> QuerySingleAsync<T>(string sql, object? param = null);
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);
    Task<int> ExecuteAsync(string sql, object? param = null);
    Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null);
}
