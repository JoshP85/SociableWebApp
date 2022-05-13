using Amazon.DynamoDBv2.DataModel;

namespace SociableWebApp.Models
{
    [DynamoDBTable("Posts")]
    public class Post
    {
        public Post()
        {
            PostID = Guid.NewGuid().ToString();
            PostDate = DateTime.Now.ToString();
            Comments = new List<Comment>();
            VoteTotal = 0;
            VoteUp = 0;
            VoteDown = 0;
        }

        [DynamoDBHashKey]
        public string PostID { get; set; }

        [DynamoDBProperty]
        public string PostAuthorID { get; set; }

        [DynamoDBProperty]
        public string PostAuthorName { get; set; }

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
        public virtual List<Comment> Comments { get; set; }

        public static Post GetPost(IDynamoDBContext dynamoDBContext, string postID)
        {
            return dynamoDBContext.LoadAsync<Post>(postID).Result;
        }

        public static async Task NewPostAsync(IDynamoDBContext dynamoDBContext, Post newPost, AppUser user)
        {
            if (newPost.PostMediaUrl == null)
            {
                newPost.PostMediaUrl = "";
            }

            Post post = new Post
            {
                PostAuthorID = user.AppUserID,
                PostAuthorName = user.Name,
                PostID = newPost.PostID,
                PostContent = newPost.PostContent,
                PostDate = newPost.PostDate,
                PostMediaUrl = newPost.PostMediaUrl,
            };
            await dynamoDBContext.SaveAsync(post);
        }
    }
}
