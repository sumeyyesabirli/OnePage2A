using OnePage2AEntity.Entites;

namespace OnePage2AClient.Models
{
    public class HomeViewModel
    {
        public List<Banner> Banner { get; set; }
        public AboutUs AboutUs { get; set; }
        public List<Services> Services { get; set; }
        public List<References> References { get; set; }
        public List<Gallery> Gallery { get; set; }
        public Contact Contact { get; set; }
    }
}
