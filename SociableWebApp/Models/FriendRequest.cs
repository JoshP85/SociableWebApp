using Amazon.DynamoDBv2.DataModel;

namespace SociableWebApp.Models
{
    public class FriendRequest
    {

        public string AppUserId { get; set; }

        public string Name { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? ProfileImgUrl { get; set; }

        public static async void CreateFriendRequest(IDynamoDBContext dynamoDBContext, string senderID, string receiverID)
        {
            AppUser sender = AppUser.GetAppUser(dynamoDBContext, senderID);
            AppUser receiver = AppUser.GetAppUser(dynamoDBContext, receiverID);

            if (senderID == receiverID)
                return;

            foreach (var sentRequest in sender.SentFriendRequests)
            {
                if (sentRequest.AppUserId == receiverID)
                    return;
            }


            FriendRequest sendFriendRequest = new()
            {
                AppUserId = receiver.AppUserID,
                Name = receiver.Name,
                City = receiver.City,
                Country = receiver.Country,
                ProfileImgUrl = receiver.ProfileImgUrl,
            };
            var requestSendList = new List<FriendRequest>
            {
                sendFriendRequest
            };

            if (sender.SentFriendRequests.Any())
                requestSendList.AddRange(sender.SentFriendRequests);

            sender.SentFriendRequests = requestSendList;

            await dynamoDBContext.SaveAsync(sender);


            FriendRequest receivedList = new()
            {
                AppUserId = sender.AppUserID,
                Name = sender.Name,
                City = sender.City,
                Country = sender.Country,
                ProfileImgUrl = sender.ProfileImgUrl,
            };

            var requestReceiveList = new List<FriendRequest>
            {
                receivedList
            };

            if (receiver.ReceivedFriendRequests != null)
                requestReceiveList.AddRange(sender.ReceivedFriendRequests);

            receiver.ReceivedFriendRequests = requestReceiveList;

            await dynamoDBContext.SaveAsync(receiver);
        }

    }
}
