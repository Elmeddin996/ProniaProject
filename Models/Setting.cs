using System.ComponentModel.DataAnnotations;

namespace ProniaProject.Models
{
    public class Setting
    {
        [Required]
        [MaxLength(25)]
        public string Key { get; set; }
        [MaxLength(250)]
        public string Value { get; set; }
    }
}
