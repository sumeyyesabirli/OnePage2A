namespace OnePage2ABussiness.AboutUs.Models
{
    public class AddAboutUsModel
    {
        public int Id { get; set; }
        public string? ImgUrl { get; set; }
        public string AboutUsDescription { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
