using OnePage2ABussiness.Users.Abstract;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2AEntity.Entites;

namespace OnePage2ABussiness.Users.Concrete
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetPagedUsersAsync(int page, int pageSize)
        {
            return await _userRepository.GetAllAsync()
                .ContinueWith(users => users.Result.Skip((page - 1) * pageSize).Take(pageSize));
        }

        public async Task<int> GetTotalUserCountAsync()
        {
            return await _userRepository.GetAllAsync()
                .ContinueWith(users => users.Result.Count());
        }

        public async Task SetActiveUserAsync(int selectedUserId)
        {
            var users = await _userRepository.GetAllAsync();
            foreach (var user in users)
            {
                user.IsActive = user.Id == selectedUserId;
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
