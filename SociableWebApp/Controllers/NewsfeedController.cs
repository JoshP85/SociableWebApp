using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
using SociableWebApp.Models;
using SociableWebApp.Session;

namespace SociableWebApp.Controllers
{
    [RequireSession]
    public class NewsfeedController : Controller
    {

        private string AppUserID => HttpContext.Session.GetString(nameof(AppUser.AppUserID));
        private readonly IDynamoDBContext dynamoDBContext;
        private readonly IAmazonDynamoDB client;

        public NewsfeedController(IDynamoDBContext dynamoDBContext, IAmazonDynamoDB client)
        {
            this.dynamoDBContext = dynamoDBContext;
            this.client = client;
        }
        public async Task<ActionResult> NewsFeedAsync()
        {
            var postList = new List<Post>();
            var conditions = new List<ScanCondition>();


            var posts = await dynamoDBContext.ScanAsync<Post>(conditions).GetRemainingAsync();
            foreach (var post in posts)
            {
                postList.Add(post);

            }


            ViewBag.Posts = postList;


            AppUser appUser = AppUser.GetAppUser(dynamoDBContext, AppUserID);
            ViewBag.AppUser = appUser;

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        // GET: NewsfeedController/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: NewsfeedController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewsfeedController/Create
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

        // GET: NewsfeedController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NewsfeedController/Edit/5
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

        // GET: NewsfeedController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NewsfeedController/Delete/5
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
