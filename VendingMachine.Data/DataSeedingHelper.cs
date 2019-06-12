using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Data
{
    public class DataSeedingHelper
    {
        public static void Seed(VmDbContext context)
        {
            context.Products.AddRange(
                new Product() { Name = "Tea", Portions = 10, Price = 1.3m },
                new Product() { Name = "Espresso", Portions = 20, Price = 1.8m },
                new Product() { Name = "Juice", Portions = 20, Price = 1.8m },
                new Product() { Name = "Chicken soup", Portions = 15, Price = 1.8m });

            context.Wallet.AddRange(
                 new Coin() { Count = 100, Value = 0.1m },
                 new Coin() { Count = 100, Value = 0.2m },
                 new Coin() { Count = 100, Value = 0.5m },
                 new Coin() { Count = 100, Value = 1.0m });

            context.SaveChanges();
        }
    }
}
