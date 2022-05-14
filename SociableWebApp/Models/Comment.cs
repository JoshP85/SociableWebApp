using Amazon.DynamoDBv2.DataModel;

namespace SociableWebApp.Models
{
    public class Comment
    {
        public Comment()
        {
            CommentID = Guid.NewGuid().ToString();
            CommentDate = DateTime.UtcNow.ToString();
        }

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

        [DynamoDBIgnore]
        public string TimeSinceComment { get; set; }

        public static async Task NewCommentAsync(IDynamoDBContext dynamoDBContext, string commentContent, AppUser user, string postID)
        {
            Comment comment = new()
            {
                CommentAuthorID = user.AppUserID,
                CommentAuthorName = user.Name,
                CommentContent = commentContent,
            };

            var commentList = new List<Comment>
            {
                comment
            };

            Post post = Post.GetPost(dynamoDBContext, postID);

            if (post.Comments.Any())
                commentList.AddRange(post.Comments);

            post.Comments = commentList;

            await dynamoDBContext.SaveAsync(post);
        }
    }
}
