using SaveWise.Model;
using SaveWise.Repositories;
using SaveWiseNew.Repositories;

namespace SaveWiseNew.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var result = await _userRepository.Add(user);
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _userRepository.Delete(id);
            return result;
        }

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
