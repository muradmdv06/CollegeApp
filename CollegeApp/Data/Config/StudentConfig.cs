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

            builder.Property(n => n.StudentName).IsRequired();
            builder.Property(n => n.StudentName).HasMaxLength(250);
            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(500);
            builder.Property(n => n.Email).IsRequired().HasMaxLength(250);

            builder.HasData(new List<Student>
    {
        new Student
        {
            Id =1,
            StudentName = "Murad",
            Email = "Murad@gmail.com",
            Address = "Baku, Azerbaijan",
            DepartmentId = 1
        },
        new Student
        {
            Id=2,
            StudentName = "Baylar",
            Email = "Baylar@gmail.com",
            Address = "Beyleqan, Azerbaijan",
            DepartmentId = 2
        }
    });

            builder.HasOne(n => n.Department)
                .WithMany(n => n.Students)
                .HasForeignKey(n => n.DepartmentId)
                .HasConstraintName("FK_Students_Department");
        }

    }
}
