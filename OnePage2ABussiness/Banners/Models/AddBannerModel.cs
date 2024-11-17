namespace OnePage2ABussiness.Banners.Models
{
    public class AddBannerModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string ImgUrl { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

    }
}
