using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace SociableWebApp.Models
{
    [DynamoDBTable("AppUsers")]
    public class AppUser
    {
        public AppUser()
        {
            AccCreatedDate = DateTime.Now.ToString();
            AppUserID = Guid.NewGuid().ToString();
            AccUpdatedDate = "";
            ReceivedFriendRequests = new List<FriendRequest> { };
            SentFriendRequests = new List<FriendRequest> { };
            Friends = new List<Friend> { };
        }

        [DynamoDBHashKey]
        public string AppUserID { get; set; }

        [DynamoDBProperty]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [DynamoDBProperty]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DynamoDBProperty]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [DynamoDBProperty]
        [Required(ErrorMessage = "Phone is required")]
        public string PhoneNumber { get; set; }

        [DynamoDBProperty]
        public string? City { get; set; }

        [DynamoDBProperty]
        public string? Country { get; set; }

        [DynamoDBProperty]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string AccCreatedDate { get; set; }

        [DynamoDBProperty]
        public string AccUpdatedDate { get; set; }

        [DynamoDBIgnore]
        public string UserImageFile { get; set; }

        [DynamoDBProperty]
        public List<Post> Posts { get; set; }

        [DynamoDBProperty]
        public virtual List<Friend> Friends { get; set; }

        [DynamoDBProperty]
        public virtual List<FriendRequest> SentFriendRequests { get; set; }

        [DynamoDBProperty]
        public virtual List<FriendRequest> ReceivedFriendRequests { get; set; }


        public static AppUser GetAppUser(IDynamoDBContext dynamoDBContext, string userID) => dynamoDBContext.LoadAsync<AppUser>(userID).Result;

        internal static async Task<bool> CreateAppUser(IDynamoDBContext dynamoDBContext, AppUser newUser)
        {
            if (GetAppUser(dynamoDBContext, newUser.Email) is not null)
                return false;

            AppUser appUser = new AppUser
            {
                Email = newUser.Email,
                Password = newUser.Password,
                Name = newUser.Name,
                PhoneNumber = newUser.PhoneNumber,
            };

            await dynamoDBContext.SaveAsync(appUser);
            return true;
        }


    }


}
