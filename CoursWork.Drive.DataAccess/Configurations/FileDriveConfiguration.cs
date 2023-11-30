using CoursWork.Drive.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursWork.Drive.DataAccess.Configurations;

public class FileDriveConfiguration : IEntityTypeConfiguration<FileDrive>
{
    public void Configure(EntityTypeBuilder<FileDrive> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        builder.Property(x => x.Description)
            .IsRequired(false);
        builder.HasOne(x => x.User)
            .WithMany(x => x.FileDrives)
            .HasForeignKey(x => x.UserId);

        builder.ToTable("Files");
    }
}
