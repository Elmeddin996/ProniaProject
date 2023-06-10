using ProniaProject.Models;

namespace ProniaProject.ViewModel
{
    public class AccountProfileViewModel
    {
        public ProfileEditViewModel Profile { get; set; }
        public List<Order> Orders { get; set; }
    }
}
