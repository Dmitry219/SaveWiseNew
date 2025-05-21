using Dapper;
using Npgsql;
using SaveWise.Model;
using SaveWiseNew.Repositories;
using System.Data;

namespace SaveWise.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<User> Add(User user)
        {
            var sql = """
                INSERT INTO users (name, age) 
                VALUES (@name, @age)
                RETURNING *
                """;

            return await _connection.QuerySingleAsync<User>(sql, user);
        }

        public async Task<List<User>> Get()
        {
            var sql = """
                SELECT * FROM users
                """;

            var result = await _connection.QueryAsync<User>(sql);
            return result.ToList();
        }

        public async Task<User?> Get(int id)
        {
            var sql = """
                SELECT * FROM users WHERE id = @id
                """;

            return await _connection.QueryFirstOrDefaultAsync<User>(sql, new { id });
        }

        public async Task<bool> Delete(int id)
        {
            var sql = """
                DELETE FROM users WHERE id = @id
                """;

            return (await _connection.ExecuteAsync(sql,
                new { id }
            )) > 0;
        }
    }
}