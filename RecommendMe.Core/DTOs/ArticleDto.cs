namespace RecommendMe.Core.DTOs
{
    public class ArticleDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Content { get; set; }
        public string Url { get; set; }
        public string? ImageUrl { get; set; }
        public double? PositivityRate { get; set; }
        public DateTime CreationDate { get; set; }
        public int SourceId { get; set; }
        public string? Source { get; set; }
    }
}
