namespace RecommendMe.Data.Entities
{
    public class RatingListMedia
    {
        public int RatingListMediaId {  get; set; }
        public int Rank { get; set; }

        public int MediaId {  get; set; }
        public Media Media { get; set; }

        public int RatingListId { get; set; }
        public RatingList RatingList { get; set; }
    }
}
