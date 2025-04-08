namespace RecommendMe.Data.Entities
{
    public class Source
    {
        public int SourceId { get; set; }
        public string SourceName { get; set; }
        public string OriginalURL { get; set; }
        public string RssUrl { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
