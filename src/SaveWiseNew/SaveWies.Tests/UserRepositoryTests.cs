using Dapper;
using Moq;
using SaveWise.Model;
using SaveWise.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveWies.Tests
{
    public class UserRepositoryTests
    {
        [Test]
        public async Task Add_ShuldCallInsertQuery_AndSetFields()
        {
            var mockConnection = new Mock<IDbConnection>();
            var user = new User { Name = "Di", Age = 12 };
            var returnedUser = new User {Id = 123, Name = "Di" , Age = 12 , DteCreate = DateTime.UtcNow};

            mockConnection.Setup(c => c.QuerySingleAsync<User>(
                It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(returnedUser);

            var repo = new UserRepository(mockConnection.Object);

            await repo.Add(user);

            Assert.AreEqual(123, user.Id);
            Assert.AreEqual(returnedUser.DteCreate, user.DteCreate);
        }
    }
}
