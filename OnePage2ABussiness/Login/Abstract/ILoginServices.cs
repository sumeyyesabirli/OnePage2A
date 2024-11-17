using OnePage2ABussiness.Login.Model;
using OnePage2AEntity.Entites;

namespace OnePage2ABussiness.Login.Abstract
{
    public interface ILoginServices
    {
        Task<User> AuthenticateAsync(AddLoginModel loginModel, string password);
        Task ResetPasswordAsync(string email);
        Task ChangePasswordAsync(string token, string newPassword);
    }
}
