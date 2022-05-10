using Amazon.DynamoDBv2.DataModel;

namespace SociableWebApp.Models
{
    public class AppUserPost
    {
        public AppUserPost()
        {
            PostID = Guid.NewGuid().ToString();
            PostDate = DateTime.Now.ToString();
            VoteTotal = 0;
            VoteUp = 0;
            VoteDown = 0;
        }

        [DynamoDBHashKey]
        public string PostID { get; set; }

        [DynamoDBProperty]
        public string AppUserID { get; set; }

        [DynamoDBProperty]
        public string PostContent { get; set; }

        [DynamoDBProperty]
        public string PostDate { get; set; }

        [DynamoDBProperty]
        public string? PostMediaUrl { get; set; }

        [DynamoDBProperty]
        public int VoteTotal { get; set; }

        [DynamoDBProperty]
        public int VoteUp { get; set; }

        [DynamoDBProperty]
        public int VoteDown { get; set; }

        [DynamoDBProperty]
        public virtual List<PostComment> Comments { get; set; }
    }
}
