namespace ReccomendMe.Data.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public double PositivityRate { get; set; }
        public DateTime CreationDate { get; set; }

        public int SourceId { get; set; }
        public Source Source { get; set; }
    }
}
