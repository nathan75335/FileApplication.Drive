using CoursWork.Drive.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursWork.Drive.DataAccess;

public class DriveContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<FileDrive> FileDrives { get; set; }
   
    public DriveContext(DbContextOptions<DriveContext> options) : base(options)
    {
       
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DriveContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
