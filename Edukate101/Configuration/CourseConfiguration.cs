using Edukate101.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edukate101.Configuration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(x=>x.ImagePath).IsRequired();
            builder.Property(x=>x.Title).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Rating).IsRequired();

            builder.ToTable(option =>
            {
                option.HasCheckConstraint("Ck_Courses_Rating", "[Rating] between 0 and 5");
            });

        }
    }
}
