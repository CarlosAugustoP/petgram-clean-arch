using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.NotificationAggregate;
using Domain.Models.UserAggregate;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.DB
{
    public class MainDBContext : DbContext
    {
        public MainDBContext(DbContextOptions<MainDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Moment> Moments { get; set; }
        public DbSet<UserBan> UserBans { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<Moment>()
                .HasOne(m => m.Author)
                .WithMany(u => u.Moments)
                .HasForeignKey(m => m.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Media>()
                .HasOne(m => m.Post)
                .WithMany(p => p.Medias)
                .HasForeignKey(m => m.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Media>()
                .HasMany(m => m.MentionedPets)
                .WithMany(p => p.Medias);
        }

    }
}
