using System.ComponentModel.DataAnnotations;

namespace ProniaProject.Areas.Manage.ViewModels
{
    public class AdminRegisterViewModel
    {
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
