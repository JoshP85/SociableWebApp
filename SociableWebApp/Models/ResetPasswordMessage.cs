using Amazon.DynamoDBv2.DataModel;

namespace SociableWebApp.Models
{
    [DynamoDBTable("resetpassword")]
    public class ResetPasswordMessage
    {
        [DynamoDBHashKey("appuserid")]
        public string AppuserID { get; set; }

        [DynamoDBProperty("email")]
        public string Email { get; set; }

        [DynamoDBProperty("name")]
        public string Name { get; set; }

        [DynamoDBProperty("code")]
        public string Code { get; set; }
    }
}
