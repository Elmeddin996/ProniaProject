using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProniaProject.Attributes.ValidationAttributes;

namespace ProniaProject.Models
{
    public class Plant
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public int CategorieId { get; set; }
        [MaxLength(500)]
        public string Desc { get; set; }
        [Column(TypeName = "money")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }
        
        [Required]
        public bool StockStatus { get; set; }
        [Required]
        public bool IsFeatured { get; set; }
        [Required]
        public bool IsNew { get; set; }
        [MaxFileSize(2097152)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        [NotMapped]
        public IFormFile PosterImage { get; set; }
        [MaxFileSize(2097152)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        [NotMapped]
        public IFormFile HoverPosterImage { get; set; }
        [NotMapped]
        [MaxFileSize(2097152)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        [NotMapped]
        public List<int> TagIds { get; set; } = new List<int>();
        [NotMapped]
        public List<int> PlantImageIds { get; set; } = new List<int>();



        public Categorie Categories { get; set; }

        public List<PlantTag> PlantTags { get; set; } = new List<PlantTag>();
        public List<PlantImage> PlantImages { get; set; } = new List<PlantImage>();
    }
}
