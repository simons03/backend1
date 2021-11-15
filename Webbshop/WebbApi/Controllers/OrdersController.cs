using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebbApi.Data;
using WebbApi.Entities;
using WebbApi.Models.Order;

namespace WebbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly SqlContext _context;

        public OrdersController(SqlContext context)
        {
            _context = context;
        }



        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult> GetOrders()
        {

            var Allorders = await _context.Orders.Include(x => x.OderLines).ToListAsync();

            var orders = new List<GetOrders>();

            foreach(var item in Allorders)
            {
                var order = new GetOrders()
                {
                    Id = item.Id,
                    UsersId = item.UsersId,
                    OrderDate = item.OrderDate,
                    Status = item.Status,
                    UserAddressesId = item.UserAddressesId,
                    TotalAmount = item.TotalAmount
                };

                foreach(var orderline in item.OderLines)
                {
                    order.OderLines.Add(new GetOrderLineModel
                    {
                        Id = orderline.Id,
                        OrdersId = orderline.OrdersId,
                        ProductsId = orderline.ProductsId,
                        Quantity = orderline.Quantity,
                        UnitPrice = orderline.UnitPrice
                    });
                }

                orders.Add(order);
            }

            return new OkObjectResult(orders);

        }






        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.Include(x => x.OderLines).FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

           
            var _order = new GetOrders()
            {
                Id = order.Id,
                UsersId = order.UsersId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                UserAddressesId = order.UserAddressesId,
                TotalAmount = order.TotalAmount
            };

            foreach (var orderline in order.OderLines)
            {
                _order.OderLines.Add(new GetOrderLineModel
                {
                    Id = orderline.Id,
                    OrdersId = orderline.OrdersId,
                    ProductsId = orderline.ProductsId,
                    Quantity = orderline.Quantity,
                    UnitPrice = orderline.UnitPrice
                });
            }

               
   
            return new OkObjectResult(_order);
        }















        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }











        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(AddOrder model)
        {
            var user = await _context.Users.FindAsync(model.UsersId);


            var order = new Order
            {
                UsersId = user.Id,
                OrderDate = DateTime.Now,
                Status = "Recieved",
                UserAddressesId = user.UserAddressesId,
                TotalAmount = model.TotalAmount,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();


            var orderLines = new List<OderLine>();
            foreach (var item in model.CartProducts)
            {
                orderLines.Add(new OderLine {
                    OrdersId = order.Id,
                    ProductsId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                });
            }

            _context.OderLines.AddRange(orderLines);
            await _context.SaveChangesAsync();

            // _context.OderLines.Add(new OderLine { OrdersId = order.Id, ProductsId });

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }












        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
