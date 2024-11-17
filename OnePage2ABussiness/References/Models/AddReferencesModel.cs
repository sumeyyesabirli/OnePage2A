namespace OnePage2ABussiness.References.Models
{
    public class AddReferencesModel
    {
        public int Id { get; set; }
        public string? ReferemcesTitle { get; set; }
        public string ImgUrl { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
