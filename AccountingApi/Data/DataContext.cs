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
    }
}
    