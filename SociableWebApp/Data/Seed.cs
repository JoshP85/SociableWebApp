using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using SociableWebApp.Models;

namespace SociableWebApp.Data
{
    public class Seed
    {
        public static async Task SeedDate(IAmazonDynamoDB client, IAmazonS3 clientS3)
        {
            var text = File.ReadAllText(@"userSeed.json");
            dynamic? data = JsonConvert.DeserializeObject(text);

            if (data is null)
            {
                return;
            }
            foreach (var item in data)
            {
                string profileImgurl = item.profileImgUrl.ToString();
                string userID = item.id.ToString();

                await UploadToS3(profileImgurl, userID, clientS3);

                DynamoDBContext context = new DynamoDBContext(client);
                AppUser appUser = new AppUser
                {
                    AppUserID = item.id,
                    Email = item.email,
                    Password = item.password,
                    Name = item.name,
                    PhoneNumber = item.phone,
                    City = item.city,
                    Country = item.country,
                    AccCreatedDate = item.accCreatedDate,
                    AccUpdatedDate = item.accUpdatedDate,
                    ReceivedFriendRequests = new List<FriendRequest> { },
                    SentFriendRequests = new List<FriendRequest> { },
                    Friends = new List<Friend> { },
                    PostIDs = new List<string> { },
                };
                await context.SaveAsync(appUser);
            }
            return;
        }

        public static async Task UploadToS3(string profileImgurl, string userID, IAmazonS3 clientS3)
        {
            var wc = new HttpClient();
            Stream fileStream = await wc.GetStreamAsync(profileImgurl);

            byte[] fileBytes = ToArrayBytes(fileStream);

            var request = new PutObjectRequest
            {
                CannedACL = S3CannedACL.PublicRead,
                BucketName = "userprofileimgs3655612",
                Key = userID,
                ContentType = "image/jpeg",
                InputStream = new MemoryStream(fileBytes)
            };
            PutObjectResponse response = await clientS3.PutObjectAsync(request);
        }

        public static byte[] ToArrayBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }
}
