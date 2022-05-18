using SociableWebApp.Models;

namespace SociableWebApp.ViewModels
{
    public class NewsFeedViewModel
    {

        public List<Post> Posts { get; set; }

        public AppUser AppUser { get; set; }

        public string PostContent { get; set; }
        public string? PostMediaUrl { get; set; }
    }
}
