using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.S3.Model;
using SociableWebApp.ExtensionMethods;

namespace SociableWebApp.Models
{
    [DynamoDBTable("Posts")]
    public class Post
    {
        public Post()
        {
            PostID = Guid.NewGuid().ToString();
            PostDate = DateTime.UtcNow.ToString();
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

        [DynamoDBIgnore]
        public string TimeSincePost { get; set; }

        [DynamoDBProperty]
        public string? PostMediaUrl { get; set; }

        [DynamoDBIgnore]
        public IFormFile MessageImageFile { get; set; }

        [DynamoDBProperty]
        public bool PostHasImage { get; set; }

        [DynamoDBProperty]
        public int VoteTotal { get; set; }

        [DynamoDBProperty]
        public int VoteUp { get; set; }

        [DynamoDBProperty]
        public int VoteDown { get; set; }

        [DynamoDBProperty]
        public virtual List<Comment> Comments { get; set; }

        public static Post GetPost(IDynamoDBContext dynamoDBContext, string postID) => dynamoDBContext.LoadAsync<Post>(postID).Result;

        public static async Task NewPostAsync(IDynamoDBContext dynamoDBContext, Post newPost, AppUser user, IAmazonS3 clientS3)
        {
            if (newPost.PostMediaUrl == null)
                newPost.PostMediaUrl = "";

            bool hasImage = newPost.MessageImageFile != null;

            Post post = new()
            {
                PostAuthorID = user.AppUserID,
                PostAuthorName = user.Name,
                PostID = newPost.PostID,
                PostContent = newPost.PostContent,
                PostDate = newPost.PostDate,
                PostMediaUrl = newPost.PostMediaUrl,
                PostHasImage = hasImage,
            };

            await dynamoDBContext.SaveAsync(post);

            user.PostIDs.Add(post.PostID);

            await dynamoDBContext.SaveAsync(user);

            if (newPost.MessageImageFile != null)
            {
                Stream fileStream = newPost.MessageImageFile.OpenReadStream();

                var request = new PutObjectRequest
                {
                    CannedACL = S3CannedACL.PublicRead,
                    BucketName = "postimages3655612",
                    Key = post.PostID,
                    ContentType = "image/jpeg",
                    InputStream = fileStream,
                };
                PutObjectResponse response = await clientS3.PutObjectAsync(request);
            }
        }

        public static List<Post> SortAndDatePosts(List<Post> posts)
        {
            var postList = new List<Post>();

            posts.Sort((x, y) => -x.PostDate.ConvertStringToDateTime().CompareTo(y.PostDate.ConvertStringToDateTime()));

            foreach (var post in posts)
            {
                post.TimeSincePost = post.PostDate.GetTimeSince(DateTime.UtcNow);

                foreach (var comment in post.Comments)
                {
                    comment.TimeSinceComment = comment.CommentDate.GetTimeSince(DateTime.UtcNow);

                }
                post.Comments.Sort((x, y) => x.CommentDate.ConvertStringToDateTime().CompareTo(y.CommentDate.ConvertStringToDateTime()));

                postList.Add(post);
            }

            return postList;
        }
    }
}
