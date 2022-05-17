using Amazon.DynamoDBv2.DataModel;

namespace SociableWebApp.Models
{
    public class Friend
    {
        public string FriendID { get; set; }


        public static void AddNewFriend(IDynamoDBContext dynamoDBContext, string newFriendID, string appUserID)
        {

        }
    }
}
