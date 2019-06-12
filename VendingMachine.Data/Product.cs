using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Data
{
    public class Product: Base
    {
        public Product()
        {

        }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Portions { get; set; }
    }
}
