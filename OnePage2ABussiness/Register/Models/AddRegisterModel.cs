using System.ComponentModel.DataAnnotations;

namespace OnePage2ABussiness.Register.Models
{
    public class AddRegisterModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Kullanıcı Adı")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Şifre Tekrar")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
    }
}
