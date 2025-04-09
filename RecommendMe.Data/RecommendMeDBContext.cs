using Microsoft.EntityFrameworkCore;
using RecommendMe.Data.Entities;

namespace RecommendMe.Data
{
    public class RecommendMeDBContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<CommentRating> CommentRating { get; set; }
        public DbSet<Form> Form { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<RatingList> RatingList { get; set; }
        public DbSet<RatingListComment> RatingListComment { get; set; }
        public DbSet<RatingListMedia> RatingListMedia { get; set; }
        public DbSet<RatingListRating> RatingListRating { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public RecommendMeDBContext(DbContextOptions<RecommendMeDBContext> options) 
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
