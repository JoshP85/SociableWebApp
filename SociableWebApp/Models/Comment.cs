using Amazon.DynamoDBv2.DataModel;

namespace SociableWebApp.Models
{
    public class Comment
    {
        public Comment()
        {
            CommentID = Guid.NewGuid().ToString();
            CommentDate = DateTime.Now.ToString();
        }

        /*        [DynamoDBProperty]
                public string PostID { get; set; }*/

        [DynamoDBHashKey]
        public string CommentAuthorID { get; set; }

        [DynamoDBProperty]
        public string CommentAuthorName { get; set; }

        [DynamoDBProperty]
        public string CommentID { get; set; }

        [DynamoDBProperty]
        public string CommentContent { get; set; }

        [DynamoDBProperty]
        public string CommentDate { get; set; }

    }
}
