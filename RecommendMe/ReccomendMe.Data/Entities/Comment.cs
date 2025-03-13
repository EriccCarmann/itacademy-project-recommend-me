using RecommendMe.Data.Entities;

namespace RecommendMe.Data.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int OriginalCommentId { get; set; }
        public Comment OriginalComment { get; set; }
    }
}
