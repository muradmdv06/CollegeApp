﻿// <auto-generated />
using System;
using CollegeApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CollegeApp.Migrations
{
    [DbContext(typeof(CollegeDBContext))]
    [Migration("20250303213005_AddingFK_Users_UserTypes")]
    partial class AddingFK_Users_UserTypes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CollegeApp.Data.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("CollegeApp.Data.RolePrivelege", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("RolePrivelegeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePriveleges", (string)null);
                });

            modelBuilder.Entity("CollegeApp.Data.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("StudentName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Students", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Baku, Azerbaijan",
                            Email = "Murad@gmail.com",
                            StudentName = "Murad"
                        },
                        new
                        {
                            Id = 2,
                            Address = "Beyleqan, Azerbaijan",
                            Email = "Baylar@gmail.com",
                            StudentName = "Baylar"
                        });
                });

            modelBuilder.Entity("CollegeApp.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PasswordSalt")
                        .HasColumnType("int");

                    b.Property<int>("Username")
                        .HasColumnType("int");

                    b.Property<int>("UsertypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsertypeId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("CollegeApp.Data.UserRoleMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex(new[] { "UserId", "RoleId" }, "UK_UserRoleMapping")
                        .IsUnique();

                    b.ToTable("UserRoleMapping", (string)null);
                });

            modelBuilder.Entity("CollegeApp.Data.UserType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("UserTypes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "for Students",
                            Name = "Student"
                        },
                        new
                        {
                            Id = 2,
                            Description = "for Students",
                            Name = "Faculty"
                        },
                        new
                        {
                            Id = 3,
                            Description = "for Supporting Staff",
                            Name = "Supporting Staff"
                        },
                        new
                        {
                            Id = 4,
                            Description = "for Parents",
                            Name = "Parents"
                        });
                });

            modelBuilder.Entity("CollegeApp.Data.RolePrivelege", b =>
                {
                    b.HasOne("CollegeApp.Data.Role", "Role")
                        .WithMany("RolePriveleges")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_RolePriveleges_Roles");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("CollegeApp.Data.User", b =>
                {
                    b.HasOne("CollegeApp.Data.UserType", "UserType")
                        .WithMany("Users")
                        .HasForeignKey("UsertypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Users_UserTypes");

                    b.Navigation("UserType");
                });

            modelBuilder.Entity("CollegeApp.Data.UserRoleMapping", b =>
                {
                    b.HasOne("CollegeApp.Data.Role", "Role")
                        .WithMany("UserRoleMappings")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_UserRoleMapping_Role");

                    b.HasOne("CollegeApp.Data.User", "User")
                        .WithMany("UserRoleMappings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_UserRoleMapping_User");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CollegeApp.Data.Role", b =>
                {
                    b.Navigation("RolePriveleges");

                    b.Navigation("UserRoleMappings");
                });

            modelBuilder.Entity("CollegeApp.Data.User", b =>
                {
                    b.Navigation("UserRoleMappings");
                });

            modelBuilder.Entity("CollegeApp.Data.UserType", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
