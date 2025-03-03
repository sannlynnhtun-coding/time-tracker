using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace TimeTracker.Database;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ClientId { get; set; } // Foreign key
    public Client Client { get; set; } // Navigation property
    public List<Task> Tasks { get; set; } = new List<Task>();
    public decimal BillableRate { get; set; }
}

public class Task
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProjectId { get; set; } // Foreign key
    public Project Project { get; set; } // Navigation property
}

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Project> Projects { get; set; } = new List<Project>();
}

public class Organization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<User> Members { get; set; } = new List<User>();
    public List<Project> Projects { get; set; } = new List<Project>();
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public decimal BillableRate { get; set; }
}

public class TimeEntry
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign key
    public User User { get; set; } // Navigation property
    public int TaskId { get; set; } // Foreign key
    public Task Task { get; set; } // Navigation property
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal BillableAmount => (decimal)(EndTime - StartTime).TotalHours * User.BillableRate;
}

public class TimeTrackerContext : DbContext
{
    private readonly string _connectionString;

    public TimeTrackerContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public TimeTrackerContext(DbContextOptions<TimeTrackerContext> options) : base(options)
    {
    }

    protected TimeTrackerContext()
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<TimeEntry> TimeEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_connectionString is not null)
            optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Client)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.ClientId);

        modelBuilder.Entity<Task>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId);

        modelBuilder.Entity<TimeEntry>()
            .HasOne(te => te.User)
            .WithMany()
            .HasForeignKey(te => te.UserId);

        modelBuilder.Entity<TimeEntry>()
            .HasOne(te => te.Task)
            .WithMany()
            .HasForeignKey(te => te.TaskId);
    }
}
