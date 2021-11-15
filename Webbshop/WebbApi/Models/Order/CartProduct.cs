using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebbApi.Models.Order
{
    public class CartProduct
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }

    }
}
