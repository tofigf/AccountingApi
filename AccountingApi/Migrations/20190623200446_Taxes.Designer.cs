﻿// <auto-generated />
using System;
using AccountingApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AccountingApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20190623200446_Taxes")]
    partial class Taxes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AccountingApi.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Behance")
                        .HasMaxLength(30);

                    b.Property<string>("City")
                        .HasMaxLength(50);

                    b.Property<string>("CompanyName")
                        .HasMaxLength(75);

                    b.Property<string>("Country")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .HasMaxLength(100);

                    b.Property<string>("Facebok")
                        .HasMaxLength(30);

                    b.Property<string>("FieldOfActivity")
                        .HasMaxLength(75);

                    b.Property<string>("Instagram")
                        .HasMaxLength(30);

                    b.Property<bool>("IsCompany");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Linkedin")
                        .HasMaxLength(30);

                    b.Property<string>("Mobile")
                        .HasMaxLength(20);

                    b.Property<string>("Name")
                        .HasMaxLength(75);

                    b.Property<string>("Phone")
                        .HasMaxLength(20);

                    b.Property<string>("PhotoFile");

                    b.Property<string>("PhotoUrl")
                        .HasMaxLength(150);

                    b.Property<string>("Postion")
                        .HasMaxLength(75);

                    b.Property<string>("Street")
                        .HasMaxLength(75);

                    b.Property<string>("Surname")
                        .HasMaxLength(75);

                    b.Property<int>("UserId");

                    b.Property<string>("VOEN")
                        .HasMaxLength(100);

                    b.Property<string>("Website")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("AccountingApi.Models.Tax", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .HasMaxLength(75);

                    b.Property<double>("Rate");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Taxes");
                });

            modelBuilder.Entity("AccountingApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasMaxLength(50);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .HasMaxLength(75);

                    b.Property<string>("Password")
                        .HasMaxLength(150);

                    b.Property<bool>("Status");

                    b.Property<string>("SurName")
                        .HasMaxLength(100);

                    b.Property<string>("Token")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AccountingApi.Models.Company", b =>
                {
                    b.HasOne("AccountingApi.Models.User", "User")
                        .WithMany("Companies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AccountingApi.Models.Tax", b =>
                {
                    b.HasOne("AccountingApi.Models.Company", "Company")
                        .WithMany("Taxes")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
