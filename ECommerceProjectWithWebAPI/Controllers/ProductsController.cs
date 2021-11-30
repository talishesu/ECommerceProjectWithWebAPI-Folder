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
    public class ProductsController : ControllerBase
    {
        private readonly ECommerceProjectWithWebAPIDbContext _context;

        public ProductsController(ECommerceProjectWithWebAPIDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.Where(p=>p.IsDeleted == false).ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.IsDeleted == true)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            if (product.Files == null && !product.Files.Any())
            {
                return Ok("Sekil Elave Olunmuyub");
            };

            if (product.Brand != null)
            {
                var brands = await _context.Brands.ToListAsync();
                brands.FirstOrDefault(b => b.Equals(product.Brand));
                return BadRequest();
            }
            var products = await _context.Products.ToListAsync();

            var oldProduct = products.Find(p => p.Name == product.Name);

            var sameProduct = products.FirstOrDefault(p => p.Id == id);

            if (oldProduct == null)
            {
                if (product.IsDeleted == true)
                {
                    return NotFound();
                }
                _context.Entry(product).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id))
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
                return Ok("Bu Adi ]Istifade Eden Product Var");
            }
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (product.Files == null && !product.Files.Any())
            {
                return Ok("Sekil Elave Olunmuyub");
            };


            if(product.Brand != null)
            {
                var brands = await _context.Brands.ToListAsync();
                brands.FirstOrDefault(b=>b.Equals(product.Brand));
                return BadRequest();
            }
            

            var products = await _context.Products.ToListAsync();

            var oldProduct = products.Find(c => c.Name == product.Name);
            if (oldProduct == null)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (oldProduct.IsDeleted == true)
                {
                    oldProduct.IsDeleted = false;
                    oldProduct.DeletedDate = null;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Ok("Bu AdiIstifade Eden Product Var");
                }
            }

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (product.IsDeleted == true)
            {
                return Ok("Bele Bir Product Yoxdur(IsDeleted=True)");
            }
            product.IsDeleted = true;
            product.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
