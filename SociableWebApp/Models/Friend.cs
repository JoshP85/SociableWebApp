using Amazon.DynamoDBv2.DataModel;

namespace SociableWebApp.Models
{
    public class Friend
    {
        public string FriendID { get; set; }


        public static async Task AddNewFriendAsync(IDynamoDBContext dynamoDBContext, string senderID, string receiverID)
        {
            AppUser sender = AppUser.GetAppUser(dynamoDBContext, senderID);
            AppUser receiver = AppUser.GetAppUser(dynamoDBContext, receiverID);

            Friend friend = new()
            {
                FriendID = sender.AppUserID,
            };

            receiver.Friends.Add(friend);

            await dynamoDBContext.SaveAsync(receiver);


            friend = new Friend()
            {
                FriendID = receiver.AppUserID,
            };

            sender.Friends.Add(friend);

            await dynamoDBContext.SaveAsync(sender);

            await FriendRequest.RemoveRequestAsync(dynamoDBContext, senderID, receiverID);
        }
    }
}
