namespace OnePage2AEntity.Entites
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? TokenExpiryDate { get; set; }
        public string Role { get; set; }
    }
}