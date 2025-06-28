using Dapper;
using SaveWiseNew.DataAccess.Interfaces;
using SaveWiseNew.DataAccess.Models;
using SaveWiseNew.Services.Interfaces;

namespace SaveWise.Repositories;

public class UserRepository(ISqlExecutor sql) : IUserRepository
{
    private readonly ISqlExecutor _sql = sql;

    /*public UserRepository(ISqlExecutor sql)
    {
        _sql = sql;
    }*/

    public async Task<User> Add(User user)
    {
        var sql = """
            INSERT INTO users (name, age) 
            VALUES (@name, @age)
            RETURNING id, name, age, date_created AS DateCreate
            """;

        return await _sql.QuerySingleAsync<User>(sql, user);
    }

    public async Task<List<User>> Get()
    {
        var sql = """
            SELECT * FROM users
            """;

        var result = await _sql.QueryAsync<User>(sql);
        return result.AsList();
    }

    public async Task<User?> Get(int id)
    {
        var sql = """
            SELECT * FROM users WHERE id = @id
            """;

        return await _sql.QueryFirstOrDefaultAsync<User>(sql, new { id });
    }

    public async Task<bool> Delete(int id)
    {
        var sql = """
            DELETE FROM users WHERE id = @id
            """;

        return (await _sql.ExecuteAsync(sql,
            new { id }
        )) > 0;
    }
}