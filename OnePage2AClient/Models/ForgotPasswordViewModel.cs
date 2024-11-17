using System.ComponentModel.DataAnnotations;

namespace OnePage2AClient.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-posta Adresi")]
        public string Email { get; set; }
    }
}
