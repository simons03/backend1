using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebbApi.Entities;

namespace WebbApi.Models.Order
{
    public class AddOrder
    {
        // public int Id { get; set; }
        public int UsersId { get; set; }
        // public DateTime OrderDate => DateTime.Now;
        // public string Status => "Recieved";
        // public int UserAddressesId { get; set; }
        public decimal TotalAmount { get; set; }


        public List<CartProduct> CartProducts { get; set; }
    }
}
