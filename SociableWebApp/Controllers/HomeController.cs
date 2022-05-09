using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using SociableWebApp.Data;
using SociableWebApp.Models;
using System.Diagnostics;

namespace SociableWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDynamoDBContext dynamoDBContext;
        private readonly IAmazonDynamoDB client;
        private readonly IAmazonS3 clientS3;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IAmazonDynamoDB client, IDynamoDBContext dynamoDBContext, IAmazonS3 clientS3, ILogger<HomeController> logger)
        {
            this.client = client;
            this.dynamoDBContext = dynamoDBContext;
            this.clientS3 = clientS3;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            await Database.CreateDatabaseAsync(client, clientS3);

            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            if (email is not null)
            {
                AppUser user = AppUser.GetAppUser(dynamoDBContext, email);
                if (user is not null)
                {
                    if (password == user.Password)
                    {
                        HttpContext.Session.SetString(nameof(AppUser.Email), user.Email);
                        return RedirectToAction("NewsFeed", "NewsFeed");
                    }
                }
            }

            ModelState.AddModelError("LoginFailed", "Email or Password is incorrect.");
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([Bind("Email, FirstName, LastName, PhoneNumber, Location, Password")] AppUser newUser)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}