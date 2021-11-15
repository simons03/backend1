using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebbApi.Entities;

namespace WebbApi.Models.Order
{
    public class GetOrderLineModel
    {
        public int Id { get; set; }
        public int OrdersId { get; set; }
        public int ProductsId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // public virtual Product Products { get; set; }


    }
}
