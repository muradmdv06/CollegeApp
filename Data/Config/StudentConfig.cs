using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.StudentName).IsRequired().HasMaxLength(250);
            builder.Property(n => n.Address).HasMaxLength(500);
            builder.Property(n => n.Email).IsRequired().HasMaxLength(250);

            builder.HasData(new List<Student>
            {
                new Student
                {
                    Id = 1,
                    StudentName = "Murad",
                    Email = "Murad@gmail.com",
                    Address = "Baku, Azerbaijan"
                },
                new Student
                {
                    Id = 2,
                    StudentName = "Baylar",
                    Email = "Baylar@gmail.com",
                    Address = "Beyleqan, Azerbaijan"
                }
            });
        }
    }
}
