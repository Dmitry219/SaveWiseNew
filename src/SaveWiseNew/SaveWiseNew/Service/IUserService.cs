using SaveWise.Model;

namespace SaveWiseNew.Service
{
    public interface IUserService
    {
        Task<User> Add(User user);
        Task<List<User>> Get();
        Task<User?> Get(int id);
        Task<bool> Delete(int id);
    }
}
