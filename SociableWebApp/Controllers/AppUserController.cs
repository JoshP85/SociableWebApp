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
            return View(AppUser.GetAppUser(dynamoDBContext, AppUserID));
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

        public ActionResult FriendRequestAccepted(string newFriendID, string approvedByID)
        {
            return View();
        }

        // POST: AppUserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AppUserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AppUserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AppUserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AppUserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
