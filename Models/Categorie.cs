using System.ComponentModel.DataAnnotations;

namespace ProniaProject.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Not longer than 20!")]
        public string Name { get; set; }

        public List<Plant> Plants { get; set; } = new List<Plant>();
    }
}
