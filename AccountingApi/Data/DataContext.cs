using AccountingApi.Models;
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

    }
}
    