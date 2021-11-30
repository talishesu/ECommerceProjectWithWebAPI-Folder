using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceProjectWithWebAPI.Models.DataContext;
using ECommerceProjectWithWebAPI.Models.Entities;

namespace ECommerceProjectWithWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ECommerceProjectWithWebAPIDbContext _context;

        public OrdersController(ECommerceProjectWithWebAPIDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.Where(p => p.IsDeleted == false).ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.IsDeleted == true)
            {
                return NotFound();
            }

            return order;
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

            if (order.Product == null)
            {
                return BadRequest();
            }
            var orders = await _context.Orders.ToListAsync();

            var oldOrder = orders.Find(o => o.Id == order.Id);

            var sameOrder = orders.FirstOrDefault(o => o.Id == id);

            if (oldOrder == null)
            {
                if (order.IsDeleted == true)
                {
                    return NotFound();
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
            else
            {
                return Ok("Bu Adi Istifade Eden Product Var");
            }
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {


            if (order.Product == null)
            {
                return BadRequest();
            }


            var products = await _context.Products.ToListAsync();

            var oldOrder = products.Find(o => o.Id == order.Id);
            if (oldOrder == null)
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (oldOrder.IsDeleted == true)
                {
                    oldOrder.IsDeleted = false;
                    oldOrder.DeletedDate = null;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Ok("Order Yaratmaq Mumkun Olmadi");
                }
            }

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
            if (order.IsDeleted == true)
            {
                return Ok("Bele Bir Sifaris Yoxdur(IsDeleted=True)");
            }
            order.IsDeleted = true;
            order.DeletedTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
