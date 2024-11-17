namespace OnePage2ABussiness.Banners.Models
{
    public class UpdateBannerModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string ImgUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
