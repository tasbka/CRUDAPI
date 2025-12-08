using DataAccess.Comments;
using DataAccess.Users;
using Microsoft.EntityFrameworkCore;
//using DataAccess.Notes;

namespace DataAccess;



public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    
    public DbSet<User> Users { get; set; }
    public DbSet<Category.Category> Categories { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }
    
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User  конфигурация   
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });
        
        // Category  конфигурация
        modelBuilder.Entity<Category.Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Name).IsUnique();
        });
        
        // Конфигурация для Note
        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(n => n.Id);
            //entity.Property(n => n.Content).HasMaxLength(100);
            
            entity.HasOne(p => p.Category)
                .WithMany(c => c.Notes)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            //связь заметок с пользователем (многие-к-одному)
            entity.HasOne(n => n.Author)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        // PostLike конфигурация
        modelBuilder.Entity<PostLike>(entity =>
        {
            entity.HasKey(pl => pl.Id);
            
            entity.HasOne(pl => pl.Note)
                .WithMany(p => p.Likes)
                .HasForeignKey(pl => pl.NoteId)
                .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(pl => pl.User)
                .WithMany(u => u.PostLikes)
                .HasForeignKey(pl => pl.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasIndex(pl => new { pl.NoteId, pl.UserId }).IsUnique();
        });
        
        // Comment конфигурация
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);
            
            entity.HasOne(c => c.Note)
                .WithMany(n => n.Comments)
                .HasForeignKey(c => c.NoteId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(c => c.ParentComment)
                .WithMany(pc => pc.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(c => c.NoteId);
            entity.HasIndex(c => c.AuthorId);
            entity.HasIndex(c => c.CreatedAt);
        });


        modelBuilder.Entity<Category.Category>().HasData(
            new Category.Category { Id = Guid.NewGuid(), Name = "API Docs", Description = "Обсуждения web API и разработки", OrderIndex = 1 },
            new Category.Category { Id = Guid.NewGuid(), Name = "Обсуждения", Description = "Общие обсуждения", OrderIndex = 2 },
            new Category.Category { Id = Guid.NewGuid(), Name = "Вопросы", Description = "Задавайте вопросы", OrderIndex = 3 },
            new Category.Category { Id = Guid.NewGuid(), Name = "Идеи", Description = "Предложения и идеи", OrderIndex = 4 }
        );
        
        base.OnModelCreating(modelBuilder);
    }
}