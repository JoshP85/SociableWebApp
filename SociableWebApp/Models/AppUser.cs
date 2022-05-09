using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace SociableWebApp.Models
{
    public class AppUser
    {
        public AppUser()
        {
            AccCreatedDate = DateTime.Now;
            AppUserID = new Guid().ToString();
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
        public DateTime AccCreatedDate { get; set; }

        [DynamoDBProperty]
        public DateTime? AccUpdatedDate { get; set; }

    }
}
