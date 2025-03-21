namespace RecommendMe.Data.Entities
{
    public class Form
    {
        public int FormId { get; set; }
        public string Topic { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
