using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Data
{
    public class Coin: Base
    {
        public decimal Value { get; set; }

        public int Count { get; set; }
    }
}
