using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Data;
using Xunit;
using System.Linq;

namespace VendingMachine.Services.Tests
{
    public class VendingMachineServiceTest
    {
        // more tests can be added
        [Fact]
        public async Task ReturnCorrectAmount()
        {
            // setup in-memory database
            var dbContextOptions = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<VmDbContext>().UseInMemoryDatabase("VMDB");            
            var dbContext = new VmDbContext(dbContextOptions.Options);
            // seed some data
            DataSeedingHelper.Seed(dbContext);

            var service = new VendingMachineService(dbContext);
            var product = (await service.GetProductsAsync()).First();            

            var change = new Dictionary<decimal, int>();
            var payCoins = new Dictionary<decimal, int>();
            payCoins.Add(0.1m, 5);
            payCoins.Add(1.0m, 2);

            var ok = await service.PurchaseAsync(product.Id, payCoins, change );

            Assert.True(ok);

            var changeSum = change.ToList().Sum(x=>x.Key * x.Value);
            var paiedSum = payCoins.ToList().Sum(x=> x.Key * x.Value);
            Assert.Equal(changeSum, paiedSum - product.Price);
        }
    }
}
