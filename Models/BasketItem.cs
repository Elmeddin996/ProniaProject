namespace ProniaProject.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public int Count { get; set; }
        public string AppUserId { get; set; }

        public Plant Plant { get; set; }
        public AppUser AppUser { get; set; }
    }
}
