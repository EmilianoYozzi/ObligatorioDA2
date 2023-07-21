﻿// <auto-generated />
using System;
using BlogDataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlogDataAccess.Migrations
{
    [DbContext(typeof(BlogApplicationContext))]
    [Migration("20230613014844_migracion2")]
    partial class migracion2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BlogDomain.Article", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<bool>("Edited")
                        .HasColumnType("bit");

                    b.Property<string>("OwnerUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Template")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Visibility")
                        .HasColumnType("int");

                    b.Property<bool>("WaitingForRevision")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("Username");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("BlogDomain.Comment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AnswerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool?>("Deleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("Edited")
                        .HasColumnType("bit");

                    b.Property<string>("IdAttachedTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("WaitingForRevision")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId")
                        .IsUnique()
                        .HasFilter("[AnswerId] IS NOT NULL");

                    b.HasIndex("ArticleId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BlogDomain.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdAttachedTo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("BlogDomain.Notification", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Username");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("BlogDomain.OffensiveWordCollection", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("offensiveWords")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OffensiveWordCollection");
                });

            modelBuilder.Entity("BlogDomain.Session", b =>
                {
                    b.Property<Guid>("Token")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Token");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("BlogDomain.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BlogDomain.Article", b =>
                {
                    b.HasOne("BlogDomain.User", null)
                        .WithMany("Articles")
                        .HasForeignKey("Username");
                });

            modelBuilder.Entity("BlogDomain.Comment", b =>
                {
                    b.HasOne("BlogDomain.Comment", "Answer")
                        .WithOne()
                        .HasForeignKey("BlogDomain.Comment", "AnswerId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("BlogDomain.Article", null)
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId");

                    b.Navigation("Answer");
                });

            modelBuilder.Entity("BlogDomain.Image", b =>
                {
                    b.HasOne("BlogDomain.Article", null)
                        .WithMany("Images")
                        .HasForeignKey("ArticleId");
                });

            modelBuilder.Entity("BlogDomain.Notification", b =>
                {
                    b.HasOne("BlogDomain.User", null)
                        .WithMany("Notifications")
                        .HasForeignKey("Username");
                });

            modelBuilder.Entity("BlogDomain.Article", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("BlogDomain.User", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
