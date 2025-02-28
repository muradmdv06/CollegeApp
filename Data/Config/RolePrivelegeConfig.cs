using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Config
{
    public class RolePrivelegeConfig : IEntityTypeConfiguration<RolePrivelege>
    {
        public void Configure(EntityTypeBuilder<RolePrivelege> builder)
        {
            builder.ToTable("RolePriveleges");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.RolePrivelegeName).IsRequired();
            builder.Property(n => n.Description).IsRequired();
            builder.Property(n => n.RoleId).IsRequired();
            builder.Property(n => n.IsActive).IsRequired();
            builder.Property(n => n.IsDeleted).IsRequired();
            builder.Property(n => n.CreatedDate).IsRequired();

            builder.HasOne(n => n.Role)
                .WithMany(n => n.RolePriveleges)
                .HasForeignKey(n => n.RoleId)
                .HasConstraintName("FK_RolePriveleges_Roles");
            



        }
    }
}
