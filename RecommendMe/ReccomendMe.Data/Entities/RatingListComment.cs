namespace RecommendMe.Data.Entities
{
    public class RatingListComment
    {
        public int RatingListCommentId { get; set; }

        public int RatingListId { get; set; }
        public RatingList RatingList { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
