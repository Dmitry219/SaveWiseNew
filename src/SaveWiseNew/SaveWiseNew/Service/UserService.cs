using SaveWise.Model;
using SaveWise.Repositories;
using SaveWiseNew.Repositories;

namespace SaveWiseNew.Service
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public Task<User> Add(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            return _userRepository.Add(user);
            
        }

        public Task<bool> Delete(int id) => _userRepository.Delete(id);

        public async Task<List<User>> Get()
        {
            var results = await _userRepository.Get();
            return results;
        }

        public async Task<User?> Get(int id)
        {
            var result = await _userRepository.Get(id);
            return result;
        }
    }
}
