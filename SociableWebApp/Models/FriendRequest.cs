using Amazon.DynamoDBv2.DataModel;

namespace SociableWebApp.Models
{
    public class FriendRequest
    {
        [DynamoDBHashKey]
        public string AppUserID { get; set; }

        public string Name { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public static async void CreateFriendRequest(IDynamoDBContext dynamoDBContext, string senderID, string receiverID)
        {
            AppUser sender = AppUser.GetAppUser(dynamoDBContext, senderID);
            AppUser receiver = AppUser.GetAppUser(dynamoDBContext, receiverID);

            // Avoids entries in the DB that would be the user requesting friendship with themselves.
            if (senderID == receiverID)
                return;

            // Avoids multiple entries of the same request in the DB.
            foreach (var sentRequest in sender.SentFriendRequests)
            {
                if (sentRequest.AppUserID == receiverID)
                    return;
            }

            // The user who sent the friend request, this updates their SentFriendRequest list
            // with the details of the user they sent the request to.
            FriendRequest detailsOfReceiver = new()
            {
                AppUserID = receiver.AppUserID,
                Name = receiver.Name,
                City = receiver.City,
                Country = receiver.Country,
            };

            sender.SentFriendRequests.Add(detailsOfReceiver);

            await dynamoDBContext.SaveAsync(sender);

            // The user who receives the friend request, this updates their ReceivedFriendRequests list
            // with the details of the user who sent the request.
            FriendRequest detailsOfSender = new()
            {
                AppUserID = sender.AppUserID,
                Name = sender.Name,
                City = sender.City,
                Country = sender.Country,
            };

            receiver.ReceivedFriendRequests.Add(detailsOfSender);

            await dynamoDBContext.SaveAsync(receiver);
        }

        public static async Task RemoveRequestAsync(IDynamoDBContext dynamoDBContext, string senderID, string receiverID)
        {
            AppUser sender = AppUser.GetAppUser(dynamoDBContext, senderID);
            AppUser receiver = AppUser.GetAppUser(dynamoDBContext, receiverID);


            foreach (var request in sender.SentFriendRequests.ToList())
            {
                if (request.AppUserID == receiverID)
                {
                    sender.SentFriendRequests.Remove(request);
                    await dynamoDBContext.SaveAsync(sender);
                }
            }

            foreach (var request in receiver.ReceivedFriendRequests.ToList())
            {
                if (request.AppUserID == senderID)
                {
                    receiver.ReceivedFriendRequests.Remove(request);
                    await dynamoDBContext.SaveAsync(receiver);
                }
            }
        }
    }
}
