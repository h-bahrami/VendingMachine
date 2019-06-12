using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Models
{
    public class OrderModel
    {
        public Guid ProductId { get; set; }

        public IEnumerable<CoinModel> Coins { get; set; }
    }
}
