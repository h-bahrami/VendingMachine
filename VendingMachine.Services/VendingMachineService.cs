using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Data;
using System.Linq;

namespace VendingMachine.Services
{
    public class VendingMachineService : IVendingMachineService
    {
        private readonly VmDbContext dbContext;

        public VendingMachineService(VmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Get accepted coins, I made an assumption that accepted coins are those in wallet 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Coin>> GetAcceptedCoinsAsync()
        {
            return await dbContext.Wallet.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await dbContext.Products.ToListAsync();
        }

        public async Task<bool> PurchaseAsync(Guid productId, IDictionary<decimal, int> coins,
            IDictionary<decimal, int> returnings)
        {
            var product = await dbContext.Products.SingleOrDefaultAsync(x => x.Id == productId);

            var amount = 0.0m;
            coins.ToList().ForEach(item => amount += (item.Key * item.Value));

            if (amount < product.Price)
            {
                return false;
            }
            else
            {
                product.Portions -= 1;
                dbContext.Products.Update(product);
                if (amount == product.Price)
                {
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var diff = amount - product.Price;
                    var availabelCoins = await dbContext.Wallet.Where(x => x.Value <= diff && x.Count > 0)
                        .OrderByDescending(x => x.Value).ToListAsync();

                    var sum = 0.0m;
                    while (sum <= diff)
                    {
                        foreach (var coin in availabelCoins)
                        {
                            sum += coin.Value;
                            if (sum <= diff)
                            {
                                returnings.Add(coin.Value, 1);
                                coin.Count -= 1;
                                dbContext.Wallet.Update(coin);
                            }
                        }
                    }
                    await dbContext.SaveChangesAsync();
                    return true;
                }
            }

        }
    }
}
