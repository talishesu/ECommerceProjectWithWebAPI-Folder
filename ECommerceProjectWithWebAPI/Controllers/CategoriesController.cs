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
    public class CategoriesController : ControllerBase
    {
        private readonly ECommerceProjectWithWebAPIDbContext _context;

        public CategoriesController(ECommerceProjectWithWebAPIDbContext context)
        {
            _context = context;
        }



        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.Where(u => u.IsDeleted == false).ToListAsync();
        }



        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            if (category.IsDeleted == true)
            {
                return NotFound();
            }

            return category;
        }



        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            var categories = await _context.Categories.ToListAsync();

            var oldCategory = categories.Find(c => c.Name == category.Name);

            var sameCategory = categories.FirstOrDefault(c => c.Id == id);

            if (oldCategory == null || sameCategory.Name == oldCategory.Name)
            {
                if (category.IsDeleted == true)
                {
                    return NotFound();
                }
                _context.Entry(category).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(id))
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
                return Ok("Bu Adi Istifade Eden Category Var");
            }
        }



        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            var categories = await _context.Categories.ToListAsync();

            var oldCategory = categories.Find(c => c.Name == category.Name);
            if (oldCategory == null)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (oldCategory.IsDeleted == true)
                {
                    oldCategory.IsDeleted = false;
                    oldCategory.DeletedTime = null;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Ok("Bu Adi Istifade Eden Category Var");
                }
            }

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }



        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            if (category.IsDeleted == true)
            {
                return Ok("Bele Bir Category Yoxdur(IsDeleted=True)");
            }
            category.IsDeleted = true;
            category.DeletedTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
