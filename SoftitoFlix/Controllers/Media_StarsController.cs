
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Models;

namespace SoftitoFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Media_StarsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public Media_StarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Media_Stars
        [HttpGet]
        public ActionResult<List<Media_Star>> GetMedia_Stars()
        {
          if (_context.Media_Stars == null)
          {
              return NotFound();
          }
            return _context.Media_Stars.ToList();
        }

        // GET: api/Media_Stars/5
        [HttpGet("{id}")]
        public ActionResult<List<Media_Star>> Media_Stars(int mediaId)
        {
            List<Media_Star> media_Stars = _context.Media_Stars.Where(mr => mr.MediaId == mediaId).ToList();
            if (media_Stars == null)
            {
                return NotFound();
            }
            return media_Stars;
        }

        [HttpGet("{id}")]
        public ActionResult<List<Media_Star>> Stars_Media(int starId)
        {
            List<Media_Star> Stars_Media = _context.Media_Stars.Where(mr => mr.StarId == starId).ToList();
            if (Stars_Media == null)
            {
                return NotFound();
            }
            return Stars_Media;
        }

        // PUT: api/Media_Stars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedia_Star(int id, Media_Star media_Star)
        {
            if (id != media_Star.MediaId)
            {
                return BadRequest();
            }

            _context.Entry(media_Star).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Media_StarExists(id))
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

        // POST: api/Media_Stars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Media_Star>> PostMedia_Star(Media_Star media_Star)
        {
          if (_context.Media_Stars == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Media_Stars'  is null.");
          }
            _context.Media_Stars.Add(media_Star);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Media_StarExists(media_Star.MediaId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMedia_Star", new { id = media_Star.MediaId }, media_Star);
        }

        // DELETE: api/Media_Stars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedia_Star(int id)
        {
            if (_context.Media_Stars == null)
            {
                return NotFound();
            }
            var media_Star = await _context.Media_Stars.FindAsync(id);
            if (media_Star == null)
            {
                return NotFound();
            }

            _context.Media_Stars.Remove(media_Star);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Media_StarExists(int id)
        {
            return (_context.Media_Stars?.Any(e => e.MediaId == id)).GetValueOrDefault();
        }
    }
}
