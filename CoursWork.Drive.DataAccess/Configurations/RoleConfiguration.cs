using CoursWork.Drive.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursWork.Drive.DataAccess.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(new List<Role>
        {
            new()
            {
                Name= "User",
                Description="this is the user role",
                Id = -1
            },
            new()
            {
                Name= "Admin",
                Description="this is the Admin Role",
                Id = -2
            }
        });

        builder.ToTable("Roles");
    }
}
