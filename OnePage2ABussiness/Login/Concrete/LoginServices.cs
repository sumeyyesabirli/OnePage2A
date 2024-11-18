using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OnePage2ABussiness.Login.Abstract;
using OnePage2ABussiness.Login.Model;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2AEntity.Entites;
using System.Net;
using System.Net.Mail;

namespace OnePage2ABussiness.Services.Concrete
{
    public class LoginServices : ILoginServices
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public LoginServices(IRepository<User> userRepository, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<User> AuthenticateAsync(AddLoginModel loginModel, string password)
        {
            var user = (await _userRepository.GetAllAsync(u => u.Name == loginModel.Name)).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Kullanıcı adı veya şifre hatalı.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result != PasswordVerificationResult.Success)
            {
                throw new Exception("Kullanıcı adı veya şifre hatalı.");
            }

            return user;
        }

        public async Task ResetPasswordAsync(string email)
        {
            var user = (await _userRepository.GetAllAsync(u => u.Email == email)).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Bu e-posta adresiyle kayıtlı bir kullanıcı bulunamadı.");
            }

            // Token oluştur ve veritabanına kaydet
            user.PasswordResetToken = Guid.NewGuid().ToString();
            user.TokenExpiryDate = DateTime.UtcNow.AddHours(1);
            await _userRepository.UpdateAsync(user);

            // Şifre sıfırlama linkini oluştur
            var resetLink = $"https://localhost:7208/Login/ResetPassword?token={user.PasswordResetToken}";
            SendEmail(user.Email, "Şifre Sıfırlama", $"Şifre sıfırlamak için tıklayın: <a href='{resetLink}'>Şifre Sıfırla</a>");
        }

        public async Task ChangePasswordAsync(string token, string newPassword)
        {
            var user = (await _userRepository.GetAllAsync(u => u.PasswordResetToken == token && u.TokenExpiryDate > DateTime.UtcNow)).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Token geçersiz veya süresi dolmuş.");
            }

            user.Password = _passwordHasher.HashPassword(user, newPassword);
            user.PasswordResetToken = null;
            user.TokenExpiryDate = null;

            await _userRepository.UpdateAsync(user);
        }

        private void SendEmail(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("noreply@yourdomain.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);
            smtpClient.Send(mailMessage);
        }
    }
}
