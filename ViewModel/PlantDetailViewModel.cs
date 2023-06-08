using ProniaProject.Models;

namespace ProniaProject.ViewModel
{
    public class PlantDetailViewModel
    {
        public Plant Plant { get; set; }
        public List<Plant> RelatedPlants { get; set; }
        public PlantComment Comment { get; set; }
    }
}
