﻿using AccountingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Invoice>()
            //            .HasRequired(m => m.)
            //            .WithMany(t => t.HomeMatches)
            //            .HasForeignKey(m => m.HomeTeamId)
            //            .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Match>()
            //            .HasRequired(m => m.GuestTeam)
            //            .WithMany(t => t.AwayMatches)
            //            .HasForeignKey(m => m.GuestTeamId)
            //            .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Worker> Workers { get; internal set; }
        public DbSet<Worker_Detail> Worker_Details { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Product_Unit> Product_Units { get; set; }
        public DbSet<Contragent> Contragents { get; set; }
        public DbSet<Contragent_Detail> Contragent_Details { get; set; }
        public DbSet<AccountsPlan> AccountsPlans { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<ProposalItem> ProposalItems { get; set; }
        public DbSet<ProposalSentMail> ProposalSentMails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<InvoiceSentMail> InvoiceSentMails { get; set; }
        public DbSet<BalanceSheet> BalanceSheets { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<IncomeItem> IncomeItems { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseInvoice> ExpenseInvoices { get; set; }
        public DbSet<ExpenseInvoiceItem> ExpenseInvoiceItems { get; set; }
        public DbSet<ExpenseItem> ExpenseItems { get; set; }

    }
}
    