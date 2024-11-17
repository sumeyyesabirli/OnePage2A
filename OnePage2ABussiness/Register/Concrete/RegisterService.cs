using Microsoft.AspNetCore.Identity;
using OnePage2ABussiness.Register.Abstract;
using OnePage2ABussiness.Register.Models;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2AEntity.Entites;

namespace OnePage2ABussiness.Register.Concrete
{
    public class RegisterService : IRegisterService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterService(IRepository<User> userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task AddUserAsync(AddRegisterModel registerModel, string password, string createdByName)
        {
            // Kullanıcının mevcut olup olmadığını kontrol et
            var existingUser = await _userRepository.GetAllAsync(u => u.Name == registerModel.Name || u.Email == registerModel.Email);
            if (existingUser != null && existingUser.Any())
            {
                throw new Exception("Kullanıcı adı veya e-posta zaten kullanılıyor.");
            }

            var user = new User
            {
                Name = registerModel.Name,
                Email = registerModel.Email,
                Role = registerModel.Role ?? "editor", // Varsayılan rol "editor"
                CreatedByName = createdByName,
                Password = _passwordHasher.HashPassword(null, password)
            };

            await _userRepository.AddAsync(user);
        }
    }
}
