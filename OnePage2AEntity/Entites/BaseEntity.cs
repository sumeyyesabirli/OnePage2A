using System.ComponentModel.DataAnnotations;

namespace OnePage2AEntity.Entites
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public string CreatedByName { get; set; }

    }
}
