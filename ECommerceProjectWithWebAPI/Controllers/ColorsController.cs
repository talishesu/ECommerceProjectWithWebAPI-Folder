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
    public class ColorsController : ControllerBase
    {
        private readonly ECommerceProjectWithWebAPIDbContext _context;



        public ColorsController(ECommerceProjectWithWebAPIDbContext context)
        {
            _context = context;
        }



        // GET: api/Colors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Color>>> GetColors()
        {
            return await _context.Colors.Where(u => u.IsDeleted == false).ToListAsync();
        }



        // GET: api/Colors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> GetColor(int id)
        {
            var color = await _context.Colors.FindAsync(id);

            if (color == null)
            {
                return NotFound();
            }

            if (color.IsDeleted == true)
            {
                return NotFound();
            }

            return color;
        }



        // PUT: api/Colors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColor(int id, Color color)
        {
            if (id != color.Id)
            {
                return BadRequest();
            }

            var colors = await _context.Colors.ToListAsync();

            var oldColor = colors.Find(c => c.Name == color.Name);

            var sameColor = colors.FirstOrDefault(c => c.Id == id);

            if (oldColor == null || sameColor.Name == oldColor.Name && sameColor.HexCode == oldColor.HexCode)
            {
                if (color.IsDeleted == true)
                {
                    return NotFound();
                }
                _context.Entry(color).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorExists(id))
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
                return Ok("Bu Adi veya HexCode Istifade Eden Color Var");
            }
        }



        // POST: api/Colors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Color>> PostColor(Color color)
        {
            var colors = await _context.Colors.ToListAsync();

            var oldColor = colors.Find(c => c.Name == color.Name && c.HexCode == color.HexCode);
            if (oldColor == null)
            {
                _context.Colors.Add(color);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (oldColor.IsDeleted == true)
                {
                    oldColor.IsDeleted = false;
                    oldColor.DeletedTime = null;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Ok("Bu Adi veya HexCode Istifade Eden Color Var");
                }
            }

            return CreatedAtAction("GetColor", new { id = color.Id }, color);
        }



        // DELETE: api/Colors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null)
            {
                return NotFound();
            }
            if (color.IsDeleted == true)
            {
                return Ok("Bele Bir Color Yoxdur(IsDeleted=True)");
            }
            color.IsDeleted = true;
            color.DeletedTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool ColorExists(int id)
        {
            return _context.Colors.Any(e => e.Id == id);
        }
    }
}
