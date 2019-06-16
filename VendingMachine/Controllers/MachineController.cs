using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.Data;
using VendingMachine.Models;
using VendingMachine.Services;

namespace VendingMachine.Controllers
{
    [Route("api/[controller]")]
    public class MachineController : Controller
    {
        private readonly IVendingMachineService service;

        public MachineController(IVendingMachineService service)
        {
            this.service = service;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Products()
        {
            var products = await service.GetProductsAsync();
            var models = products.Select(x => new { x.Name, x.Portions, x.Price, x.Id }).ToList();
            return Ok(models);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> AcceptedCoins()
        {
            var coins = await service.GetAcceptedCoinsAsync();
            var models = coins.Select(x => new { x.Value }).ToList();
            return Ok(models);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Purchase([FromBody] OrderModel order)
        {
            var coins = order.Coins.Select(x => new KeyValuePair<decimal, int>(x.Value, x.Count)).ToDictionary(x => x.Key, x => x.Value);
            var returnings = new Dictionary<decimal, int>();
            var succeed = await service.PurchaseAsync(order.ProductId, coins, returnings);

            return Ok(new
            {
                Message = succeed ? "Thank you" : "Insufficient amount",
                Coins = returnings.Select(x => new CoinModel() { Value = x.Key, Count = x.Value })
            });
        }

    }
}
