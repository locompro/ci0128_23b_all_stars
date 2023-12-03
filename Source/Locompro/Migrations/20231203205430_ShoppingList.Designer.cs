﻿// <auto-generated />
using System;
using Locompro.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

#nullable disable

namespace Locompro.Migrations
{
    [DbContext(typeof(LocomproContext))]
    [Migration("20231203205430_ShoppingList")]
    partial class ShoppingList
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.Property<string>("CategoriesName")
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("ProductsId")
                        .HasColumnType("int");

                    b.HasKey("CategoriesName", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("CategoryProduct");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Canton", b =>
                {
                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProvinceName")
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Name")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("CountryName", "ProvinceName", "Name");

                    b.ToTable("Cantons");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Category", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("ParentName")
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Name");

                    b.HasIndex("ParentName");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Country", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Picture", b =>
                {
                    b.Property<string>("SubmissionUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SubmissionEntryTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<byte[]>("PictureData")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PictureTitle")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubmissionUserId", "SubmissionEntryTime", "Index");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Brand")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Model")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Province", b =>
                {
                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("CountryName", "Name");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Report", b =>
                {
                    b.Property<string>("SubmissionUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SubmissionEntryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubmissionUserId", "SubmissionEntryTime", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("Reports");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Report");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Store", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.Property<string>("CantonCountryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CantonName")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("CantonProvinceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)");

                    b.Property<Point>("Location")
                        .HasColumnType("geography");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("Name");

                    b.HasIndex("CantonCountryName", "CantonProvinceName", "CantonName");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Submission", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("EntryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("NumberOfRatings")
                        .HasColumnType("bigint");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StoreName")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("UserId", "EntryTime");

                    b.HasIndex("ProductId");

                    b.HasIndex("StoreName");

                    b.ToTable("Submissions");
                });

            modelBuilder.Entity("Locompro.Models.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Locompro.Models.Results.GetPicturesResult", b =>
                {
                    b.Property<byte[]>("PictureData")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PictureTitle")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("GetPicturesResult");
                });

            modelBuilder.Entity("Locompro.Models.Results.GetQualifiedUserIDsResult", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("GetQualifiedUserIDsResult");
                });

            modelBuilder.Entity("Locompro.Models.Results.MostReportedUsersResult", b =>
                {
                    b.Property<int>("ReportedSubmissionCount")
                        .HasColumnType("int");

                    b.Property<int>("TotalUserSubmissions")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("UserRating")
                        .HasColumnType("real");

                    b.ToTable("MostReportedUsersResult");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ShoppingList", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProductId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ShoppingList");
                });

            modelBuilder.Entity("SubmissionApprover", b =>
                {
                    b.Property<string>("ApproverId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SubmissionUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SubmissionEntryTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ApproverId", "SubmissionUserId", "SubmissionEntryTime");

                    b.HasIndex("SubmissionUserId", "SubmissionEntryTime");

                    b.ToTable("SubmissionApprover");
                });

            modelBuilder.Entity("SubmissionRejecter", b =>
                {
                    b.Property<string>("RejecterId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SubmissionUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SubmissionEntryTime")
                        .HasColumnType("datetime2");

                    b.HasKey("RejecterId", "SubmissionUserId", "SubmissionEntryTime");

                    b.HasIndex("SubmissionUserId", "SubmissionEntryTime");

                    b.ToTable("SubmissionRejecter");
                });

            modelBuilder.Entity("Locompro.Models.Entities.AutoReport", b =>
                {
                    b.HasBaseType("Locompro.Models.Entities.Report");

                    b.Property<double>("AveragePrice")
                        .HasColumnType("float");

                    b.Property<double>("Confidence")
                        .HasColumnType("float");

                    b.Property<int>("MaximumPrice")
                        .HasColumnType("int");

                    b.Property<int>("MinimumPrice")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("AutoReport");
                });

            modelBuilder.Entity("Locompro.Models.Entities.UserReport", b =>
                {
                    b.HasBaseType("Locompro.Models.Entities.Report");

                    b.HasDiscriminator().HasValue("UserReport");
                });

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.HasOne("Locompro.Models.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Locompro.Models.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Locompro.Models.Entities.Canton", b =>
                {
                    b.HasOne("Locompro.Models.Entities.Province", "Province")
                        .WithMany("Cantons")
                        .HasForeignKey("CountryName", "ProvinceName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Category", b =>
                {
                    b.HasOne("Locompro.Models.Entities.Category", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentName");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Picture", b =>
                {
                    b.HasOne("Locompro.Models.Entities.Submission", "Submission")
                        .WithMany("Pictures")
                        .HasForeignKey("SubmissionUserId", "SubmissionEntryTime")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Submission");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Province", b =>
                {
                    b.HasOne("Locompro.Models.Entities.Country", "Country")
                        .WithMany("Provinces")
                        .HasForeignKey("CountryName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Report", b =>
                {
                    b.HasOne("Locompro.Models.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Store", b =>
                {
                    b.HasOne("Locompro.Models.Entities.Canton", "Canton")
                        .WithMany()
                        .HasForeignKey("CantonCountryName", "CantonProvinceName", "CantonName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Canton");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Submission", b =>
                {
                    b.HasOne("Locompro.Models.Entities.Product", "Product")
                        .WithMany("Submissions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Locompro.Models.Entities.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Locompro.Models.Entities.User", "User")
                        .WithMany("CreatedSubmissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Store");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Locompro.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Locompro.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Locompro.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Locompro.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShoppingList", b =>
                {
                    b.HasOne("Locompro.Models.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Locompro.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SubmissionApprover", b =>
                {
                    b.HasOne("Locompro.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("ApproverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Locompro.Models.Entities.Submission", null)
                        .WithMany()
                        .HasForeignKey("SubmissionUserId", "SubmissionEntryTime")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SubmissionRejecter", b =>
                {
                    b.HasOne("Locompro.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("RejecterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Locompro.Models.Entities.Submission", null)
                        .WithMany()
                        .HasForeignKey("SubmissionUserId", "SubmissionEntryTime")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Locompro.Models.Entities.AutoReport", b =>
                {
                    b.HasOne("Locompro.Models.Entities.Submission", "Submission")
                        .WithMany("AutoReports")
                        .HasForeignKey("SubmissionUserId", "SubmissionEntryTime")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Submission");
                });

            modelBuilder.Entity("Locompro.Models.Entities.UserReport", b =>
                {
                    b.HasOne("Locompro.Models.Entities.Submission", "Submission")
                        .WithMany("UserReports")
                        .HasForeignKey("SubmissionUserId", "SubmissionEntryTime")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Submission");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Category", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Country", b =>
                {
                    b.Navigation("Provinces");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Product", b =>
                {
                    b.Navigation("Submissions");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Province", b =>
                {
                    b.Navigation("Cantons");
                });

            modelBuilder.Entity("Locompro.Models.Entities.Submission", b =>
                {
                    b.Navigation("AutoReports");

                    b.Navigation("Pictures");

                    b.Navigation("UserReports");
                });

            modelBuilder.Entity("Locompro.Models.Entities.User", b =>
                {
                    b.Navigation("CreatedSubmissions");
                });
#pragma warning restore 612, 618
        }
    }
}
