using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using SociableWebApp.ExtensionMethods;
using SociableWebApp.Models;
using SociableWebApp.Session;
using SociableWebApp.ViewModels;

namespace SociableWebApp.Controllers
{
    [RequireSession]
    public class NewsfeedController : Controller
    {

        private string AppUserID => HttpContext.Session.GetString(nameof(AppUser.AppUserID));
        private readonly IDynamoDBContext dynamoDBContext;
        private readonly IAmazonDynamoDB client;
        private readonly IAmazonS3 clientS3;

        public NewsfeedController(IDynamoDBContext dynamoDBContext, IAmazonDynamoDB client, IAmazonS3 clientS3)
        {
            this.dynamoDBContext = dynamoDBContext;
            this.client = client;
            this.clientS3 = clientS3;
        }

        public async Task<ActionResult> NewsFeedAsync()
        {
            var postList = new List<Post>();
            var conditions = new List<ScanCondition>();

            //TODO: Condition to be only friends posts are returned
            var posts = await dynamoDBContext.ScanAsync<Post>(conditions).GetRemainingAsync();

            posts.Sort((x, y) => -x.PostDate.ConvertStringToDateTime().CompareTo(y.PostDate.ConvertStringToDateTime()));

            foreach (var post in posts)
            {
                post.TimeSincePost = post.PostDate.GetTimeSince(DateTime.UtcNow);

                foreach (var comment in post.Comments)
                {
                    comment.TimeSinceComment = comment.CommentDate.GetTimeSince(DateTime.UtcNow);

                }
                post.Comments.Sort((x, y) => x.CommentDate.ConvertStringToDateTime().CompareTo(y.CommentDate.ConvertStringToDateTime()));

                postList.Add(post);
            }

            NewsFeedViewModel newsfeedVM = new NewsFeedViewModel()
            {
                Posts = postList,
                AppUser = AppUser.GetAppUser(dynamoDBContext, AppUserID)
            };

            return View(newsfeedVM);
        }

        [HttpPost]
        public async Task<ActionResult> NewPostAsync([Bind("PostContent, MessageImageFile")] Post newPost)
        {
            var user = AppUser.GetAppUser(dynamoDBContext, AppUserID);

            await Post.NewPostAsync(dynamoDBContext, newPost, user, clientS3);

            return RedirectToAction("Newsfeed", "NewsFeed");
        }

        [HttpPost]
        public async Task<ActionResult> NewCommentAsync(string commentContent, string PostID)
        {
            var user = AppUser.GetAppUser(dynamoDBContext, AppUserID);

            await Comment.NewCommentAsync(dynamoDBContext, commentContent, user, PostID);

            return RedirectToAction("Newsfeed", "NewsFeed");
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
