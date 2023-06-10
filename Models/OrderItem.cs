using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaProject.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int PlantId { get; set; }
        public int Count { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitCostPrice { get; set; }
       
        public Order Order { get; set; }
        public Plant Plant { get; set; }
    }
}
