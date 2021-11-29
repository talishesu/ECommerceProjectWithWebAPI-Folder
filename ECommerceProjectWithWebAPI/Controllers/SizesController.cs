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
    public class SizesController : ControllerBase
    {
        private readonly ECommerceProjectWithWebAPIDbContext _context;



        public SizesController(ECommerceProjectWithWebAPIDbContext context)
        {
            _context = context;
        }



        // GET: api/Sizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Size>>> GetSizes()
        {
            return await _context.Sizes.Where(u => u.IsDeleted == false).ToListAsync();
        }



        // GET: api/Sizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Size>> GetSize(int id)
        {
            var size = await _context.Sizes.FindAsync(id);

            if (size == null)
            {
                return NotFound();
            }

            if (size.IsDeleted == true)
            {
                return NotFound();
            }

            return size;
        }



        // PUT: api/Sizes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSize(int id, Size size)
        {
            if (id != size.Id)
            {
                return BadRequest();
            }

            var sizes = await _context.Sizes.ToListAsync();

            var oldSize = sizes.Find(s => s.Name == size.Name);

            var sameSize = sizes.FirstOrDefault(s => s.Id == id);

            if (oldSize == null || sameSize.Name == oldSize.Name)
            {
                if (size.IsDeleted == true)
                {
                    return NotFound();
                }
                _context.Entry(size).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(id))
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
                return Ok("Bu Adi Istifade Eden Size Var");
            }
        }



        // POST: api/Sizes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Size>> PostSize(Size size)
        {
            var sizes = await _context.Sizes.ToListAsync();

            var oldSize = sizes.Find(s => s.Name == size.Name);
            if (oldSize == null)
            {
                _context.Sizes.Add(size);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (oldSize.IsDeleted == true)
                {
                    oldSize.IsDeleted = false;
                    oldSize.DeletedTime = null;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Ok("Bu Adi Istifade Eden Size Var");
                }
            }

            return CreatedAtAction("GetSize", new { id = size.Id }, size);
        }



        // DELETE: api/Sizes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSize(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            if (size.IsDeleted == true)
            {
                return Ok("Bele Bir Size Yoxdur(IsDeleted=True)");
            }
            size.IsDeleted = true;
            size.DeletedTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool SizeExists(int id)
        {
            return _context.Sizes.Any(e => e.Id == id);
        }
    }
}
