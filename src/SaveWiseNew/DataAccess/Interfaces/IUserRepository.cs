using SaveWiseNew.DataAccess.Models;

namespace SaveWiseNew.DataAccess.Interfaces;

public interface IUserRepository
{
    Task<User> Add(User user);
    Task<List<User>> Get();
    Task<User?> Get(int id);
    Task<bool> Delete(int id);
}
