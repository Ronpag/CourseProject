using Microsoft.EntityFrameworkCore;

namespace CRM.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<TaskStatusRequest> TaskStatusRequests { get; set; }
    public DbSet<ProfileChangeRequest> ProfileChangeRequests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Crm.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Client)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Task>()
            .HasOne(t => t.Worker)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.WorkerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
