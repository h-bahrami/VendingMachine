using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Data;

namespace VendingMachine.Services
{
    public interface IVendingMachineService
    {

        Task<IEnumerable<Coin>> GetAcceptedCoinsAsync();

        Task<IEnumerable<Product>> GetProductsAsync();

        Task<bool> PurchaseAsync(Guid productId, IDictionary<decimal, int> coinsValueCountPairs, IDictionary<decimal, int> returnings);
        
    }
}
