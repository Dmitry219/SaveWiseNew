using Dapper;
using Npgsql;
using SaveWise.Model;
using System.Data;

namespace SaveWise.Repositories
{
    public class UserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task Add(User user)
        {
            var sql = """
                INSERT INTO users (name, age) 
                VALUES (@name, @age)
                returning *
                """;
                

            var result = await _connection.QuerySingleAsync<User>(sql, user);
            user.Id = result.Id;
            user.DteCreate = result.DteCreate;
        }

        public async Task<IEnumerable<User>> Get()
        {
            var sql = """
                SELECT * FROM users
                """;

            return await _connection.QueryAsync<User>(sql);
        }

        public async Task<User> Get(int idUser)
        {
            var sql = """
                SELECT * FROM users WHERE id = @idUser
                """;

            return await _connection.QueryFirstOrDefaultAsync<User>(sql,
                new { idUser }
            );
        }

        public async Task Delete(int idUser)
        {
            var sql = """
                DELETE users WHERE id = @idUser
                """;

            await _connection.ExecuteAsync(sql,
                new { Id = idUser }
            );
        }
    }
}