﻿using System;
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
    public class BrandsController : ControllerBase
    {
        private readonly ECommerceProjectWithWebAPIDbContext _context;

        public BrandsController(ECommerceProjectWithWebAPIDbContext context)
        {
            _context = context;
        }



        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            return await _context.Brands.Where(u => u.IsDeleted == false).ToListAsync();
        }



        // GET: api/Brands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {

            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            if (brand.IsDeleted == true)
            {
                return NotFound();
            }

            return brand;
        }



        // PUT: api/Brands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(int id, Brand brand)
        {
            if (id != brand.Id)
            {
                return BadRequest();
            }

            var brands = await _context.Brands.ToListAsync();

            var oldBrand = brands.Find(b => b.Name == brand.Name);

            var sameBrand = brands.FirstOrDefault(b => b.Id == id);

            if (oldBrand == null || sameBrand.Name == oldBrand.Name)
            {
                if (brand.IsDeleted == true)
                {
                    return NotFound();
                }
                _context.Entry(brand).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(id))
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
                return Ok("Bu Adi Istifade Eden Brand Var");
            }
        }



        // POST: api/Brands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            var brands = await _context.Brands.ToListAsync();

            var oldBrand = brands.Find(b => b.Name == brand.Name);
            if(oldBrand == null)
            {
                _context.Brands.Add(brand);
                await _context.SaveChangesAsync();
            }else
            {
                if (oldBrand.IsDeleted == true)
                {
                    oldBrand.IsDeleted = false;
                    oldBrand.DeletedTime = null;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Ok("Bu Adi Istifade Eden Brand Var");
                }
            }

            return CreatedAtAction("GetBrand", new { id = brand.Id }, brand);
        }



        // DELETE: api/Brands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            if (brand.IsDeleted == true)
            {
                return Ok("Bele Bir Brand Yoxdur(IsDeleted=True)");
            }
            brand.IsDeleted = true;
            brand.DeletedTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.Id == id);
        }
    }
}
