using SociableWebApp.Models;

namespace SociableWebApp.ViewModels
{
    public class PublicProfileViewModel
    {
        public AppUser OwnerOfProfile { get; set; }
        public AppUser CurrentUser { get; set; }
        public bool IsOwnerAFriend { get; set; }
        public bool IsRelationshipPending { get; set; }
        public bool IsOwnerCurrentSessionUser { get; set; }
        public bool IsRelationshipNotConfirmed { get; set; }
        public List<AppUser> OwnerOfProfileFriends { get; set; }
        public List<Post> OwnerOfProfilePosts { get; set; }
    }
}
