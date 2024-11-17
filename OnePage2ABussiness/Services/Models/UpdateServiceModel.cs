namespace OnePage2ABussiness.Services.Models
{
    public class UpdateServiceModel
    {
        public int Id { get; set; }
        public string? ImgUrl { get; set; }
        public string ServicesTitle { get; set; }
        public string? ServicesDescription { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
