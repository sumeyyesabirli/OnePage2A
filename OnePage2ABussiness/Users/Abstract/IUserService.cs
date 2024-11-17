using OnePage2AEntity.Entites;

namespace OnePage2ABussiness.Users.Abstract
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetPagedUsersAsync(int page, int pageSize);
        Task<int> GetTotalUserCountAsync();
        Task SetActiveUserAsync(int selectedUserId);
        Task DeleteUserAsync(int id);
    }
}
