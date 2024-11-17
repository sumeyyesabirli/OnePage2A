using OnePage2ABussiness.Register.Models;

namespace OnePage2ABussiness.Register.Abstract
{
    public interface IRegisterService
    {
        Task AddUserAsync(AddRegisterModel registerModel, string password, string createdByName);
    }
}

