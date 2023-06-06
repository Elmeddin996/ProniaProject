using ProniaProject.Models;

namespace ProniaProject.ViewModel
{
    public class HomeViewModel
    {
        public List<Plant> Bestsellers { get; set; }
        public List<Plant> Features { get; set; }
        public List<Plant> New { get; set; }
        public List<Slider> Sliders { get; set; }
    }
}
