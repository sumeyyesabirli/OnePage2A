using System.ComponentModel.DataAnnotations;

namespace OnePage2AClient.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
