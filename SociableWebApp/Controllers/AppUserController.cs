using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using SociableWebApp.Models;
using SociableWebApp.Session;
using SociableWebApp.ViewModels;

namespace SociableWebApp.Controllers
{
    [RequireSession]
    public class AppUserController : Controller
    {
        private string AppUserID => HttpContext.Session.GetString(nameof(AppUser.AppUserID));
        private readonly IDynamoDBContext dynamoDBContext;
        private readonly IAmazonDynamoDB client;
        private readonly IAmazonS3 clientS3;

        public AppUserController(IDynamoDBContext dynamoDBContext, IAmazonDynamoDB client, IAmazonS3 clientS3)
        {
            this.dynamoDBContext = dynamoDBContext;
            this.client = client;
            this.clientS3 = clientS3;
        }

        public ActionResult Profile(string appUserID)
        {
            if (appUserID == null && TempData["userOfProfile"] == null)
            {
                appUserID = AppUserID;
            }

            if (TempData["userOfProfile"] != null)
            {
                appUserID = TempData["userOfProfile"].ToString();
            }

            var user = AppUser.GetAppUser(dynamoDBContext, AppUserID);

            var ownerOfProfile = AppUser.GetAppUser(dynamoDBContext, appUserID);

            var friends = new List<AppUser>();

            foreach (var friend in ownerOfProfile.Friends)
            {
                friends.Add(AppUser.GetAppUser(dynamoDBContext, friend.FriendID));
            }

            var posts = new List<Post>();

            foreach (var postID in ownerOfProfile.PostIDs)
            {
                posts.Add(Post.GetPost(dynamoDBContext, postID));
            }

            var postList = Post.SortAndDatePosts(posts);

            var PublicProfileViewModel = new PublicProfileViewModel
            {
                OwnerOfProfile = ownerOfProfile,
                CurrentUser = user,
                IsOwnerAFriend = user.Friends.Any(friend => friend.FriendID == appUserID),
                IsRelationshipPending = user.SentFriendRequests.Any(x => x.AppUserID == appUserID),
                IsRelationshipNotConfirmed = user.ReceivedFriendRequests.Any(x => x.AppUserID == appUserID),
                IsOwnerCurrentSessionUser = (appUserID == AppUserID),
                OwnerOfProfileFriends = friends,
                OwnerOfProfilePosts = postList,
            };

            return View(PublicProfileViewModel);
        }

        public ActionResult SendFriendRequest(string receiverID)
        {
            FriendRequest.CreateFriendRequest(dynamoDBContext, AppUserID, receiverID);

            TempData["userOfProfile"] = receiverID;

            return RedirectToAction("Profile", "AppUser");
        }

        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> ConfirmFriendRequestAsync(string senderID, bool accept, bool fromProfile)
        {
            if (accept)
                await Friend.AddNewFriendAsync(dynamoDBContext, senderID, AppUserID);
            else
                await FriendRequest.RemoveRequestAsync(dynamoDBContext, senderID, AppUserID);

            if (fromProfile)
            {
                TempData["userOfProfile"] = senderID;
                return RedirectToAction("Profile", "AppUser");
            }

            return RedirectToAction("NewsFeed", "NewsFeed");
        }

        public async Task<ActionResult> RemovePendingRequest(string receiverID, bool fromProfile)
        {
            await FriendRequest.RemoveRequestAsync(dynamoDBContext, AppUserID, receiverID);

            if (fromProfile)
            {
                TempData["userOfProfile"] = receiverID;
                return RedirectToAction("Profile", "AppUser");
            }

            return RedirectToAction("NewsFeed", "NewsFeed");
        }

        public async Task<ActionResult> Unfriend(string removeFriendID)
        {
            await Friend.Unfriend(dynamoDBContext, AppUserID, removeFriendID);

            TempData["userOfProfile"] = removeFriendID;

            return RedirectToAction("Profile", "AppUser");
        }
    }
}
