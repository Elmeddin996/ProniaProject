using ProniaProject.Areas.Manage.ViewModels;
using ProniaProject.Models;

namespace ProniaProject.ViewModel
{
    public class ShopViewModel
    {
        public List<Categorie> Categories { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Plant> Plants { get; set; }
        public PaginatedList<Plant> PaginatedList { get; set; }
    }
}
