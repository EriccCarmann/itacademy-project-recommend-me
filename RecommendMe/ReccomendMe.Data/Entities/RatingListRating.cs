using RecommendMe.Data.Entities;

namespace RecommendMe.Data.Entities
{
    public class RatingListRating
    {
        public int RatingListRatingId { get; set; }
        public bool Like { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
