namespace DataAccess;
using Microsoft.EntityFrameworkCore;

public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    public DbSet<Note> Notes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>().HasKey(n => n.Id);
        modelBuilder.Entity<Note>().Property(n => n.Text).HasMaxLength(100);
        base.OnModelCreating(modelBuilder);
    }
}