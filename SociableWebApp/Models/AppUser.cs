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
        }

        [DynamoDBHashKey]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [DynamoDBProperty]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DynamoDBProperty]
        public string AppUserID { get; set; }

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
        public string? ProfileImgUrl { get; set; }

        [DynamoDBProperty]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string AccCreatedDate { get; set; }

        [DynamoDBProperty]
        public DateTime? AccUpdatedDate { get; set; }


        public static AppUser GetAppUser(IDynamoDBContext dynamoDBContext, string email)
        {
            return dynamoDBContext.LoadAsync<AppUser>(email).Result;
        }

        internal static async Task<bool> CreateAppUser(IDynamoDBContext dynamoDBContext, AppUser newUser)
        {
            if (GetAppUser(dynamoDBContext, newUser.Email) is not null)
            {
                return false;
            }

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
