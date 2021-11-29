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
    public class ParentChildCategoriesController : ControllerBase
    {
        private readonly ECommerceProjectWithWebAPIDbContext _context;



        public ParentChildCategoriesController(ECommerceProjectWithWebAPIDbContext context)
        {
            _context = context;
        }



        // GET: api/ParentChildCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParentChildCategory>>> GetParentChildCategories()
        {
            return await _context.ParentChildCategories.Where(u => u.IsDeleted == false).ToListAsync();
        }



        // GET: api/ParentChildCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParentChildCategory>> GetParentChildCategory(int id)
        {
            var parentChildCategory = await _context.ParentChildCategories.FindAsync(id);

            if (parentChildCategory == null)
            {
                return NotFound();
            }

            if (parentChildCategory.IsDeleted == true)
            {
                return NotFound();
            }

            return parentChildCategory;
        }



        // PUT: api/ParentChildCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParentChildCategory(int id, ParentChildCategory parentChildCategory)
        {
            if (id != parentChildCategory.Id)
            {
                return BadRequest();
            }

            var parentChildCategories = await _context.ParentChildCategories.ToListAsync();

            var oldParentChildCategory = parentChildCategories.Find(pCC => pCC.ParentCategoryId == parentChildCategory.ParentCategoryId && pCC.ChildCategoryId == parentChildCategory.ChildCategoryId);

            var sameParentChildCategory = parentChildCategories.FirstOrDefault(pCC => pCC.Id == id);

            if (oldParentChildCategory == null || sameParentChildCategory.ParentCategoryId == oldParentChildCategory.ParentCategoryId && sameParentChildCategory.ChildCategoryId == oldParentChildCategory.ChildCategoryId)
            {
                if (parentChildCategory.IsDeleted == true)
                {
                    return NotFound();
                }
                _context.Entry(parentChildCategory).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParentChildCategoryExists(id))
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
                return Ok("Bu ParentCategoryId ve ChildCategoryId Istifade Eden ParentChildCategory Var");
            }
        }



        // POST: api/ParentChildCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ParentChildCategory>> PostParentChildCategory(ParentChildCategory parentChildCategory)
        {
            var parentChildCategories = await _context.ParentChildCategories.ToListAsync();

            var oldParentChildCategory = parentChildCategories.Find(pCC => pCC.ParentCategoryId == parentChildCategory.ParentCategoryId && pCC.ChildCategoryId == parentChildCategory.ChildCategoryId);
            if (oldParentChildCategory == null)
            {
                _context.ParentChildCategories.Add(parentChildCategory);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (oldParentChildCategory.IsDeleted == true)
                {
                    oldParentChildCategory.IsDeleted = false;
                    oldParentChildCategory.DeletedTime = null;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Ok("Bu ParentCategoryId ve ChildCategoryId Istifade Eden ParentChildCategory Var");
                }
            }

            return CreatedAtAction("GetParentChildCategories", new { id = parentChildCategory.Id }, parentChildCategory);
        }



        // DELETE: api/ParentChildCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParentChildCategory(int id)
        {
            var parentChildCategory = await _context.ParentChildCategories.FindAsync(id);
            if (parentChildCategory == null)
            {
                return NotFound();
            }
            if (parentChildCategory.IsDeleted == true)
            {
                return Ok("Bele Bir ParentChildCategory Yoxdur(IsDeleted=True)");
            }
            parentChildCategory.IsDeleted = true;
            parentChildCategory.DeletedTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool ParentChildCategoryExists(int id)
        {
            return _context.ParentChildCategories.Any(e => e.Id == id);
        }
    }
}
