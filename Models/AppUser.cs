using Microsoft.AspNetCore.Identity;

namespace ProniaProject.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ConnectionId { get; set; }
    }
}
