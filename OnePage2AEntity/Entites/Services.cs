using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnePage2AEntity.Entites
{
    public class Services : BaseEntity
    {
        public string? ImgUrl { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public string ServicesTitle { get; set; }
        public string? ServicesDescription { get; set; }
    }
}
