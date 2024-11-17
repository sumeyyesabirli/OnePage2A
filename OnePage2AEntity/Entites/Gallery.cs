using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnePage2AEntity.Entites
{
    public class Gallery : BaseEntity
    {
        public string ImgUrl { get; set; }
        [NotMapped]
        public List<IFormFile> ImageFile { get; set; }
    }
}
