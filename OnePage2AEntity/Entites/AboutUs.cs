using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnePage2AEntity.Entites
{
    public class AboutUs : BaseEntity
    {
        public string? ImgUrl { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public string AboutUsDescription { get; set; }
    }
}
