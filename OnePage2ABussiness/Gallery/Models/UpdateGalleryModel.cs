namespace OnePage2ABussiness.Gallery.Models
{
    public class UpdateGalleryModel
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
