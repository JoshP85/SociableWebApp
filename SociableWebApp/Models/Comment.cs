using Amazon.DynamoDBv2.DataModel;

namespace SociableWebApp.Models
{
    public class Comment
    {
        public Comment()
        {
            CommentId = Guid.NewGuid().ToString();
            CommentDate = DateTime.Now.ToString();
        }


        /*        public string PostID { get; set; }*/

        [DynamoDBHashKey]
        public string AuthorId { get; set; }

        [DynamoDBProperty]
        public string AuthorName { get; set; }

        [DynamoDBProperty]
        public string CommentId { get; set; }

        [DynamoDBProperty]
        public string CommentContent { get; set; }

        [DynamoDBProperty]
        public string CommentDate { get; set; }

    }
}
