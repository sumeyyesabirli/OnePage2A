using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnePage2AEntity.Entites
{
    public class Banner : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string ImgUrl { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
