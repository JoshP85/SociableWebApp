using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
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
        public async Task<IActionResult> IndexAsync(string email, string password)
        {
            if (email is not null)
            {
                var request = new ScanRequest
                {
                    TableName = "AppUsers",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":val", new AttributeValue { S = email }}
                },
                    FilterExpression = "Email = :val",
                };
                ScanResponse response = await client.ScanAsync(request);

                if (response.Count > 0)
                {
                    string userID = "";
                    foreach (Dictionary<string, AttributeValue> item in response.Items)
                    {
                        userID = item["AppUserID"].S.ToString();
                    }

                    AppUser user = AppUser.GetAppUser(dynamoDBContext, userID);

                    if (user is not null)
                    {
                        if (password == user.Password)
                        {
                            HttpContext.Session.SetString(nameof(AppUser.AppUserID), user.AppUserID);
                            return RedirectToAction("NewsFeed", "NewsFeed");
                        }
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
        public async Task<IActionResult> RegisterAsync([Bind("Email, Name, PhoneNumber, Password")] AppUser newUser)
        {
            var isSuccessful = await AppUser.CreateAppUser(dynamoDBContext, newUser);
            if (isSuccessful)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("RegisterFailed", $"{newUser.Email} is already registered with an account.");
            return View();
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPasswordAsync(string email)
        {
            if (email is not null)
            {
                var request = new ScanRequest
                {
                    TableName = "AppUsers",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":val", new AttributeValue { S = email }}
                },
                    FilterExpression = "Email = :val",
                };
                ScanResponse response = await client.ScanAsync(request);

                if (response.Count > 0)
                {
                    string userID = "";
                    foreach (Dictionary<string, AttributeValue> item in response.Items)
                    {
                        userID = item["AppUserID"].S.ToString();
                    }

                    AppUser user = AppUser.GetAppUser(dynamoDBContext, userID);

                    var resetpasswordmessage = new ResetPasswordMessage
                    {
                        AppuserID = userID,
                        Email = user.Email,
                        Name = user.Name,
                        Code = "1234"
                    };
                    await dynamoDBContext.SaveAsync(resetpasswordmessage);
                    return RedirectToAction("EnterResetCode", "Home");
                }

            }
            ModelState.AddModelError("InvalidEmail", "Email does not exist in our system.");
            return View();
        }

        public IActionResult EnterResetCode()
        {
            return View();
        }

        public IActionResult ResetPassword()
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