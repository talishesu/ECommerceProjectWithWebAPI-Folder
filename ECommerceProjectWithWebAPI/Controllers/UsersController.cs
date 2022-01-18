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



        // GET: api/MyUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.MyUsers.Where(u=>u.IsDeleted == false).ToListAsync();
        }



        // GET: api/MyUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.MyUsers.FindAsync(id);

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



        // PUT: api/MyUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }



            var MyUsers = await _context.MyUsers.ToListAsync();

            var oldUser = MyUsers.Find(u => u.Email == user.Email);

            var sameUser = MyUsers.FirstOrDefault(u => u.Id == id);

            if (oldUser == null || sameUser.Email == oldUser.Email)
            {
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
            else
            {
                return Ok("Bu Email Istifade Eden User Var");
            }
            
        }



        // POST: api/MyUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var MyUsers = await _context.MyUsers.ToListAsync();

            var oldUser = MyUsers.Find(u => u.Email == user.Email);
            if(oldUser == null)
            {
                _context.MyUsers.Add(user);
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



        // DELETE: api/MyUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.MyUsers.FindAsync(id);
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
            return _context.MyUsers.Any(e => e.Id == id);
        }
    }
}
