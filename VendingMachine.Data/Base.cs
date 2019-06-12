using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Data
{
    public class Base
    {
        public Guid Id { get; set; }

        public Base()
        {
            Id = Guid.NewGuid();
        }
    }
}
