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
    public class TrackActionsController : ControllerBase
    {
        private readonly ECommerceProjectWithWebAPIDbContext _context;

        public TrackActionsController(ECommerceProjectWithWebAPIDbContext context)
        {
            _context = context;
        }

        // GET: api/TrackActions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackAction>>> GetTrackActions()
        {
            return await _context.TrackActions.Where(u => u.IsDeleted == false).ToListAsync();
        }

        // GET: api/TrackActions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackAction>> GetTrackAction(int id)
        {
            var trackAction = await _context.TrackActions.FindAsync(id);

            if (trackAction == null)
            {
                return NotFound();
            }

            if (trackAction.IsDeleted == true)
            {
                return NotFound();
            }

            return trackAction;
        }

        // PUT: api/TrackActions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrackAction(int id, TrackAction trackAction)
        {
            if (id != trackAction.Id)
            {
                return BadRequest();
            }

            var trackActions = await _context.TrackActions.ToListAsync();

            var oldTrackAction = trackActions.Find(tA => tA.Name == trackAction.Name);

            var sameTrackAction = trackActions.FirstOrDefault(tA => tA.Id == id);

            if (oldTrackAction == null || sameTrackAction.Name == oldTrackAction.Name)
            {
                if (trackAction.IsDeleted == true)
                {
                    return NotFound();
                }
                _context.Entry(trackAction).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackActionExists(id))
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
                return Ok("Bu Adi Istifade Eden TrackAction Var");
            }
        }

        // POST: api/TrackActions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrackAction>> PostTrackAction(TrackAction trackAction)
        {
            var trackActions = await _context.TrackActions.ToListAsync();

            var oldTrackAction = trackActions.Find(tA => tA.Name == trackAction.Name);
            if (oldTrackAction == null)
            {
                _context.TrackActions.Add(trackAction);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (oldTrackAction.IsDeleted == true)
                {
                    oldTrackAction.IsDeleted = false;
                    oldTrackAction.DeletedTime = null;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Ok("Bu Adi Istifade Eden TrackAction Var");
                }
            }

            return CreatedAtAction("GetTrackAction", new { id = trackAction.Id }, trackAction);
        }

        // DELETE: api/TrackActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrackAction(int id)
        {
            var trackAction = await _context.TrackActions.FindAsync(id);
            if (trackAction == null)
            {
                return NotFound();
            }
            if (trackAction.IsDeleted == true)
            {
                return Ok("Bele Bir TrackAction Yoxdur(IsDeleted=True)");
            }
            trackAction.IsDeleted = true;
            trackAction.DeletedTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrackActionExists(int id)
        {
            return _context.TrackActions.Any(e => e.Id == id);
        }
    }
}
