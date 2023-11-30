using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FileAccess = CoursWork.Drive.DataAccess.Entities.FileAccess;

namespace CoursWork.Drive.DataAccess.Configurations;

public class FileAccessConfiguration : IEntityTypeConfiguration<FileAccess>
{
    public void Configure(EntityTypeBuilder<FileAccess> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.FileDrive)
            .WithMany(x => x.FileAccesses)
            .HasForeignKey(x => x.FileDriveId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.FileAccesses)
            .HasForeignKey(x => x.UserId);

        builder.ToTable("FileAccesses");
    }
}
