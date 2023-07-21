using Microsoft.EntityFrameworkCore;
using BlogDomain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BlogDataAccess.Context
{
    public class BlogApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        public BlogApplicationContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Username);
            modelBuilder.Entity<User>().HasMany(u => u.Articles).WithOne();
            modelBuilder.Entity<User>().HasMany(u => u.Notifications).WithOne();

            modelBuilder.Entity<Article>().HasKey(a => a.Id);
            modelBuilder.Entity<Article>().HasMany(a => a.Comments).WithOne();
            modelBuilder.Entity<Article>().HasMany(a => a.Images).WithOne();

            modelBuilder.Entity<Image>().HasKey(i => i.Id);


            modelBuilder.Entity<Comment>()
                                .HasOne(c => c.Answer)
                                .WithOne()
                                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Session>().HasKey(s => s.Token);

            modelBuilder.Entity<Notification>().HasKey(a => a.Id);

            modelBuilder.Entity<OffensiveWordCollection>()
                .Property(o => o.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<OffensiveWordCollection>().HasKey(s => s.Id);
            modelBuilder.Entity<OffensiveWordCollection>().Property(o => o.offensiveWords).HasConversion(
                                                                                            v => string.Join(',', v),
                                                                                            v => v.Split(',', System.StringSplitOptions.RemoveEmptyEntries).ToList(),
                                                                                            new ValueComparer<List<string>>(
                                                                                            (c1, c2) => c1.SequenceEqual(c2),
                                                                                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                                                                            c => (List<string>)c.ToList()));            
        }
    }
}
