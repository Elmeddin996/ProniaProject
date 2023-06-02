namespace ProniaProject.Models
{
    public class PlantImage
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public string ImageName { get; set; }
        public bool? PosterStatus { get; set; }

        public Plant Plant { get; set; }
    }
}
