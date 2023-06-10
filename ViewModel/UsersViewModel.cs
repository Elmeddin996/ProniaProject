using Microsoft.AspNetCore.Identity;
using ProniaProject.Models;

namespace ProniaProject.ViewModel
{
    public class UsersViewModel
    {
        public AppUser User { get; set; }

        public string Role { get; set; }    
    }
}
