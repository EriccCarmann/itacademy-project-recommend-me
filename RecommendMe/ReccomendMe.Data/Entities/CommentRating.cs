using ReccomendMe.Data.Entities;

namespace RecommendMe.Data.Entities
{
    public class CommentRating
    {
        public int CommentRatingId { get; set; }
        public bool Like { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
