using Amazon.DynamoDBv2.DataModel;

namespace SociableWebApp.Models
{
    public class PostComment : AppUserPost
    {
        public PostComment()
        {
            CommentId = Guid.NewGuid().ToString();
            CommentDate = DateTime.Now.ToString();
        }

        [DynamoDBProperty]
        public string CommentedById { get; set; }

        [DynamoDBProperty]
        public string CommentId { get; set; }

        [DynamoDBProperty]
        public string CommentContent { get; set; }

        [DynamoDBProperty]
        public string CommentDate { get; set; }

    }
}
