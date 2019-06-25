﻿// <auto-generated />
using System;
using AccountingApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AccountingApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AccountingApi.Models.AccountsPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccPlanNumber")
                        .HasMaxLength(100);

                    b.Property<bool?>("Active");

                    b.Property<string>("Category")
                        .HasMaxLength(350);

                    b.Property<int>("CompanyId");

                    b.Property<bool?>("ContraAccount");

                    b.Property<double?>("Debit");

                    b.Property<double?>("Kredit");

                    b.Property<int?>("Level");

                    b.Property<string>("Name")
                        .HasMaxLength(350);

                    b.Property<string>("Obeysto")
                        .HasMaxLength(350);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("AccountsPlans");
                });

            modelBuilder.Entity("AccountingApi.Models.BalanceSheet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AccountsPlanId");

                    b.Property<int>("CompanyId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<double?>("DebitMoney");

                    b.Property<int?>("InvoiceId");

                    b.Property<double?>("KreditMoney");

                    b.HasKey("Id");

                    b.HasIndex("AccountsPlanId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("InvoiceId");

                    b.ToTable("BalanceSheets");
                });

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

            modelBuilder.Entity("AccountingApi.Models.Contragent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<string>("CompanyName")
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreetedAt");

                    b.Property<string>("Email")
                        .HasMaxLength(75);

                    b.Property<string>("FieldOfActivity")
                        .HasMaxLength(75);

                    b.Property<string>("Fullname")
                        .HasMaxLength(100);

                    b.Property<bool>("IsCostumer");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Phone")
                        .HasMaxLength(75);

                    b.Property<string>("PhotoFile");

                    b.Property<string>("PhotoUrl")
                        .HasMaxLength(150);

                    b.Property<string>("Position")
                        .HasMaxLength(75);

                    b.Property<string>("VOEN")
                        .HasMaxLength(75);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Contragents");
                });

            modelBuilder.Entity("AccountingApi.Models.Contragent_Detail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adress")
                        .HasMaxLength(75);

                    b.Property<string>("Behance")
                        .HasMaxLength(75);

                    b.Property<string>("City")
                        .HasMaxLength(75);

                    b.Property<int>("ContragentId");

                    b.Property<string>("Country")
                        .HasMaxLength(75);

                    b.Property<string>("Facebook")
                        .HasMaxLength(75);

                    b.Property<string>("Instagram")
                        .HasMaxLength(75);

                    b.Property<string>("Linkedin")
                        .HasMaxLength(75);

                    b.Property<string>("WebSite")
                        .HasMaxLength(75);

                    b.HasKey("Id");

                    b.HasIndex("ContragentId");

                    b.ToTable("Contragent_Details");
                });

            modelBuilder.Entity("AccountingApi.Models.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AccountDebitId");

                    b.Property<int?>("AccountKreditId");

                    b.Property<int>("CompanyId");

                    b.Property<int?>("ContragentId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Desc")
                        .HasMaxLength(300);

                    b.Property<DateTime?>("EndDate");

                    b.Property<string>("InvoiceNumber")
                        .HasMaxLength(250);

                    b.Property<bool>("IsDeleted");

                    b.Property<byte>("IsPaid");

                    b.Property<DateTime?>("PreparingDate");

                    b.Property<double?>("ResidueForCalc");

                    b.Property<double?>("Sum");

                    b.Property<int?>("TaxId");

                    b.Property<double?>("TotalPrice");

                    b.Property<double?>("TotalTax");

                    b.HasKey("Id");

                    b.HasIndex("AccountDebitId");

                    b.HasIndex("AccountKreditId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ContragentId");

                    b.HasIndex("TaxId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("AccountingApi.Models.InvoiceItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("InvoiceId");

                    b.Property<double?>("Price");

                    b.Property<int?>("ProductId");

                    b.Property<int?>("Qty");

                    b.Property<double?>("SellPrice");

                    b.Property<double?>("TotalOneProduct");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.HasIndex("ProductId");

                    b.ToTable("InvoiceItems");
                });

            modelBuilder.Entity("AccountingApi.Models.InvoiceSentMail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasMaxLength(50);

                    b.Property<int>("InvoiceId");

                    b.Property<byte>("IsPaid");

                    b.Property<string>("Token")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.ToTable("InvoiceSentMails");
                });

            modelBuilder.Entity("AccountingApi.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .HasMaxLength(75);

                    b.Property<int>("CompanyId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsServiceOrProduct");

                    b.Property<string>("Name")
                        .HasMaxLength(75);

                    b.Property<string>("PhotoFile");

                    b.Property<string>("PhotoUrl")
                        .HasMaxLength(150);

                    b.Property<int?>("UnitId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("UnitId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("AccountingApi.Models.Product_Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CompanyId");

                    b.Property<string>("Name")
                        .HasMaxLength(75);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Product_Units");
                });

            modelBuilder.Entity("AccountingApi.Models.Proposal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<int?>("ContragentId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Desc")
                        .HasMaxLength(300);

                    b.Property<DateTime?>("EndDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("PreparingDate");

                    b.Property<string>("ProposalNumber")
                        .HasMaxLength(500);

                    b.Property<double?>("Sum");

                    b.Property<int?>("TaxId");

                    b.Property<double?>("TotalPrice");

                    b.Property<double?>("TotalTax");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ContragentId");

                    b.HasIndex("TaxId");

                    b.ToTable("Proposals");
                });

            modelBuilder.Entity("AccountingApi.Models.ProposalItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("Price");

                    b.Property<int?>("ProductId");

                    b.Property<int?>("ProposalId");

                    b.Property<int?>("Qty");

                    b.Property<double?>("SellPrice");

                    b.Property<double?>("TotalOneProduct");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProposalId");

                    b.ToTable("ProposalItems");
                });

            modelBuilder.Entity("AccountingApi.Models.ProposalSentMail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasMaxLength(50);

                    b.Property<byte>("IsPaid");

                    b.Property<int>("ProposalId");

                    b.Property<string>("Token")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("ProposalId");

                    b.ToTable("ProposalSentMails");
                });

            modelBuilder.Entity("AccountingApi.Models.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Count");

                    b.Property<string>("Desc")
                        .HasMaxLength(275);

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPurchase");

                    b.Property<bool>("IsSale");

                    b.Property<double?>("Price");

                    b.Property<int>("ProductId");

                    b.Property<double?>("SalePrice");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Stocks");
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

            modelBuilder.Entity("AccountingApi.Models.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<string>("Departament")
                        .HasMaxLength(75);

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsState");

                    b.Property<string>("Name")
                        .HasMaxLength(75);

                    b.Property<string>("PartofDepartament")
                        .HasMaxLength(75);

                    b.Property<string>("PhotoFile");

                    b.Property<string>("PhotoUrl");

                    b.Property<string>("Positon")
                        .HasMaxLength(75);

                    b.Property<DateTime>("RegisterDate");

                    b.Property<string>("Role")
                        .HasMaxLength(75);

                    b.Property<double?>("Salary");

                    b.Property<string>("SurName")
                        .HasMaxLength(75);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("AccountingApi.Models.Worker_Detail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adress")
                        .HasMaxLength(75);

                    b.Property<DateTime?>("Birthday");

                    b.Property<string>("DSMF")
                        .HasMaxLength(75);

                    b.Property<string>("Education")
                        .HasMaxLength(75);

                    b.Property<string>("EducationLevel")
                        .HasMaxLength(75);

                    b.Property<string>("Email")
                        .HasMaxLength(75);

                    b.Property<string>("FatherName")
                        .HasMaxLength(75);

                    b.Property<string>("Gender")
                        .HasMaxLength(20);

                    b.Property<string>("MobilePhone")
                        .HasMaxLength(75);

                    b.Property<string>("Phone")
                        .HasMaxLength(75);

                    b.Property<string>("Voen")
                        .HasMaxLength(75);

                    b.Property<int>("WorkerId");

                    b.HasKey("Id");

                    b.HasIndex("WorkerId");

                    b.ToTable("Worker_Details");
                });

            modelBuilder.Entity("AccountingApi.Models.AccountsPlan", b =>
                {
                    b.HasOne("AccountingApi.Models.Company", "Company")
                        .WithMany("AccountsPlans")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AccountingApi.Models.BalanceSheet", b =>
                {
                    b.HasOne("AccountingApi.Models.AccountsPlan", "AccountsPlan")
                        .WithMany("BalanceSheets")
                        .HasForeignKey("AccountsPlanId");

                    b.HasOne("AccountingApi.Models.Company", "Company")
                        .WithMany("BalanceSheets")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AccountingApi.Models.Invoice", "Invoice")
                        .WithMany("BalanceSheets")
                        .HasForeignKey("InvoiceId");
                });

            modelBuilder.Entity("AccountingApi.Models.Company", b =>
                {
                    b.HasOne("AccountingApi.Models.User", "User")
                        .WithMany("Companies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AccountingApi.Models.Contragent", b =>
                {
                    b.HasOne("AccountingApi.Models.Company", "Company")
                        .WithMany("Contragents")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AccountingApi.Models.Contragent_Detail", b =>
                {
                    b.HasOne("AccountingApi.Models.Contragent", "Contragent")
                        .WithMany("Contragent_Details")
                        .HasForeignKey("ContragentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AccountingApi.Models.Invoice", b =>
                {
                    b.HasOne("AccountingApi.Models.AccountsPlan", "AccountsPlanDebit")
                        .WithMany("InvoicesDebit")
                        .HasForeignKey("AccountDebitId");

                    b.HasOne("AccountingApi.Models.AccountsPlan", "AccountsPlanKredit")
                        .WithMany("InvoicesKredit")
                        .HasForeignKey("AccountKreditId");

                    b.HasOne("AccountingApi.Models.Company", "Company")
                        .WithMany("Invoices")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AccountingApi.Models.Contragent", "Contragent")
                        .WithMany("Invoices")
                        .HasForeignKey("ContragentId");

                    b.HasOne("AccountingApi.Models.Tax", "Tax")
                        .WithMany()
                        .HasForeignKey("TaxId");
                });

            modelBuilder.Entity("AccountingApi.Models.InvoiceItem", b =>
                {
                    b.HasOne("AccountingApi.Models.Invoice", "Invoice")
                        .WithMany("InvoiceItems")
                        .HasForeignKey("InvoiceId");

                    b.HasOne("AccountingApi.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("AccountingApi.Models.InvoiceSentMail", b =>
                {
                    b.HasOne("AccountingApi.Models.Invoice", "Invoice")
                        .WithMany("InvoiceSentMails")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AccountingApi.Models.Product", b =>
                {
                    b.HasOne("AccountingApi.Models.Company", "Company")
                        .WithMany("Products")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AccountingApi.Models.Product_Unit", "Unit")
                        .WithMany("Products")
                        .HasForeignKey("UnitId");
                });

            modelBuilder.Entity("AccountingApi.Models.Product_Unit", b =>
                {
                    b.HasOne("AccountingApi.Models.Company", "Company")
                        .WithMany("Product_Units")
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("AccountingApi.Models.Proposal", b =>
                {
                    b.HasOne("AccountingApi.Models.Company", "Company")
                        .WithMany("Proposals")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AccountingApi.Models.Contragent", "Contragent")
                        .WithMany("Proposals")
                        .HasForeignKey("ContragentId");

                    b.HasOne("AccountingApi.Models.Tax", "Tax")
                        .WithMany("Proposals")
                        .HasForeignKey("TaxId");
                });

            modelBuilder.Entity("AccountingApi.Models.ProposalItem", b =>
                {
                    b.HasOne("AccountingApi.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("AccountingApi.Models.Proposal", "Proposal")
                        .WithMany("ProposalItems")
                        .HasForeignKey("ProposalId");
                });

            modelBuilder.Entity("AccountingApi.Models.ProposalSentMail", b =>
                {
                    b.HasOne("AccountingApi.Models.Proposal", "Proposal")
                        .WithMany("ProposalSentMails")
                        .HasForeignKey("ProposalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AccountingApi.Models.Stock", b =>
                {
                    b.HasOne("AccountingApi.Models.Product", "Product")
                        .WithMany("Stocks")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AccountingApi.Models.Tax", b =>
                {
                    b.HasOne("AccountingApi.Models.Company", "Company")
                        .WithMany("Taxes")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AccountingApi.Models.Worker", b =>
                {
                    b.HasOne("AccountingApi.Models.Company", "Company")
                        .WithMany("Workers")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AccountingApi.Models.Worker_Detail", b =>
                {
                    b.HasOne("AccountingApi.Models.Worker", "Worker")
                        .WithMany("Worker_Details")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
