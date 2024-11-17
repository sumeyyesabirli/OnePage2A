using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnePage2AEntity.Entites
{
    public class References : BaseEntity
    {
        public string? ReferemcesTitle { get; set; }
        public string ImgUrl { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
