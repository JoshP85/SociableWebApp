using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using SociableWebApp.Models;

namespace SociableWebApp.Controllers
{
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


        public ActionResult Profile()
        {
            var appuser = AppUser.GetAppUser(dynamoDBContext, AppUserID);
            var friends = new List<AppUser>();

            foreach (var friend in appuser.Friends)
            {
                friends.Add(AppUser.GetAppUser(dynamoDBContext, friend.FriendID));
            }

            ViewBag.Friends = friends;

            return View(appuser);
        }

        public ActionResult PublicProfile()
        {
            return View(AppUser.GetAppUser(dynamoDBContext, AppUserID));
        }

        [HttpPost]
        public ActionResult PublicProfile(string appUserID)
        {
            return View(AppUser.GetAppUser(dynamoDBContext, appUserID));
        }

        public ActionResult SendFriendRequest(string receiverID)
        {
            FriendRequest.CreateFriendRequest(dynamoDBContext, AppUserID, receiverID);
            // string senderID = AppUserID;
            return RedirectToAction("NewsFeed", "NewsFeed");
        }

        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> ConfirmFriendRequestAsync(string senderID, bool accept)
        {
            if (accept)
                await Friend.AddNewFriendAsync(dynamoDBContext, senderID, AppUserID);
            else
                await FriendRequest.RemoveRequestAsync(dynamoDBContext, senderID, AppUserID);

            return RedirectToAction("NewsFeed", "NewsFeed");
        }

        public async Task<ActionResult> RemovePendingRequest(string receiverID)
        {
            await FriendRequest.RemoveRequestAsync(dynamoDBContext, AppUserID, receiverID);

            return RedirectToAction("NewsFeed", "NewsFeed");
        }

        public async Task<ActionResult> Unfriend(string removeFriendID)
        {
            await Friend.Unfriend(dynamoDBContext, AppUserID, removeFriendID);

            return RedirectToAction("Profile", "AppUser");
        }
    }
}
