namespace RecommendMe.Data.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public DateTime Expires { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;

        public bool IsRevoked { get; set; }

        public bool IsActive => !IsExpired && !IsRevoked;

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
