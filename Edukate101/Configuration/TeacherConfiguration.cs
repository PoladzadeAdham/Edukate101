using Edukate101.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edukate101.Configuration
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(256);

            builder.Property(x => x.ImagePath).IsRequired();

            builder.HasOne(x=>x.Course).WithMany(x=>x.Teachers).HasForeignKey(x=>x.CourseId);

        }


    }
}
