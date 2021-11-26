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
    public class UsersController : ControllerBase
    {
        private readonly ECommerceProjectWithWebAPIDbContext _context;

        public UsersController(ECommerceProjectWithWebAPIDbContext context)
        {
            _context = context;
        }
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.Where(u=>u.IsDeleted == false).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.IsDeleted == true)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            if (user.IsDeleted == true)
            {
                return NotFound();
            }
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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



        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var users = await _context.Users.ToListAsync();

            var oldUser = users.Find(u => u.Email == user.Email);
            if(oldUser == null)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }else
            {
                if (oldUser.IsDeleted == true)
                {
                    oldUser.IsDeleted = false;
                    oldUser.DeletedTime = null;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Ok("Bu Email Istifade Eden User Var");
                }
            }

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.IsDeleted == true)
            {
                return Ok("Bele Bir Istifadeci Yoxdur(IsDeleted=True)");
            }
            user.IsDeleted = true;
            user.DeletedTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
