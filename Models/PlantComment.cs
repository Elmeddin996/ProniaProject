using System.ComponentModel.DataAnnotations;

namespace ProniaProject.Models
{
    public class PlantComment
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public int PlantId { get; set; }
        [Required]
        [MaxLength(500)]
        public string Text { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rate { get; set; }
        public DateTime CreatedAt { get; set; }

        public AppUser AppUser { get; set; }
        public Plant Plant { get; set; }
    }
}
