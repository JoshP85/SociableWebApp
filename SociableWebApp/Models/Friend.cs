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

        public static async Task Unfriend(IDynamoDBContext dynamoDBContext, string appUserID, string removeFriendID)
        {
            AppUser user = AppUser.GetAppUser(dynamoDBContext, appUserID);
            AppUser removedUser = AppUser.GetAppUser(dynamoDBContext, removeFriendID);

            foreach (var friendID in user.Friends.ToList())
            {
                if (friendID.FriendID == removedUser.AppUserID)
                {
                    user.Friends.Remove(friendID);
                    await dynamoDBContext.SaveAsync(user);
                }
            }

            foreach (var friendID in removedUser.Friends.ToList())
            {
                if (friendID.FriendID == user.AppUserID)
                {
                    removedUser.Friends.Remove(friendID);
                    await dynamoDBContext.SaveAsync(removedUser);
                }
            }
        }
    }
}
