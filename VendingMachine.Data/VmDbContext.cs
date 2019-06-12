using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Data
{
    public class VmDbContext : DbContext
    {
        public VmDbContext()
        {
        }

        public VmDbContext(DbContextOptions<VmDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Coin> Wallet { get; set; }
      
    }
}
