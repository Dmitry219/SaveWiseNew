using Dapper;
using Moq;
using Npgsql;
using SaveWise.Model;
using SaveWise.Repositories;
using SaveWiseNew.Repositories;
using SaveWiseNew.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveWies.Tests
{
    [TestFixture]
    public class UserServiceTest
    {
        private IDbConnection _connection;
        private IUserRepository _repository;
        private IUserService _service;

        [SetUp]
        public void SetUp()
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1111;Database=SaveWise";
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();

            _repository = new UserRepository(_connection);
            _service = new UserService(_repository);

            _connection.Execute("DELETE FROM users");
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Close();
        }

        [Test]
        public async Task Add_ShouldAddUser()
        {
            var user = new User { Name = "ServiceUser", Age = 33 };

            var result = await _service.Add(user);

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.GreaterThan(0));
            Assert.That(result.Name, Is.EqualTo("ServiceUser"));
        }

        [Test]
        public void Add_ShouldThrowArgumentNullException_WhenUserIsNull()
        {
            User nullUser = null;

            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.Add(nullUser));
        }

        [Test]
        public async Task Get_ShouldReturnAllUsers()
        {
            await _service.Add(new User { Name = "A", Age = 20 });
            await _service.Add(new User { Name = "B", Age = 21 });

            var result = await _service.Get();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Get_ById_ShouldReturnCorrectUser()
        {
            var added = await _service.Add(new User { Name = "ById", Age = 40 });

            var result = await _service.Get(added.Id);

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("ById"));
        }

        [Test]
        public async Task Delete_ShouldRemoveUser()
        {
            var added = await _service.Add(new User { Name = "ToDelete", Age = 50 });

            var deleted = await _service.Delete(added.Id);
            var result = await _service.Get(added.Id);

            Assert.IsTrue(deleted);
            Assert.IsNull(result);
        }
    }
}
