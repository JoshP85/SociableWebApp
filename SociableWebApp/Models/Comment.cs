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

        public static async Task NewCommentAsync(IDynamoDBContext dynamoDBContext, string commentContent, AppUser user, string postID)
        {
            Comment comment = new Comment
            {
                CommentAuthorID = user.AppUserID,
                CommentAuthorName = user.Name,
                CommentContent = commentContent,
            };

            var commentList = new List<Comment>();
            commentList.Add(comment);
            Post post = Post.GetPost(dynamoDBContext, postID);

            if (post.Comments.Any())
            {
                commentList.AddRange(post.Comments);
            }
            post.Comments = commentList;

            await dynamoDBContext.SaveAsync(post);
        }
    }
}
