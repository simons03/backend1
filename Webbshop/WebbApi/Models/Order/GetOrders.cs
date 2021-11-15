using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebbApi.Entities;

namespace WebbApi.Models.Order
{
    public class GetOrders
    {

        public int Id { get; set; }
        public int UsersId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public int UserAddressesId { get; set; }
        public decimal TotalAmount { get; set; }

        public List<GetOrderLineModel> OderLines { get; set; } = new();


    }
}
