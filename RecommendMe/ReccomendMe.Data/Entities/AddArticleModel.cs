namespace RecommendMe.Data.Entities
{
    public class AddArticleModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public double PositivityRate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
