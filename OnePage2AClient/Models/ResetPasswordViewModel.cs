using System.ComponentModel.DataAnnotations;

namespace OnePage2AClient.Models
{
    public class ResetPasswordViewModel
    {
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Yeni Şifre Tekrar")]
        public string ConfirmPassword { get; set; }
    }
}
