
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Models;

namespace SoftitoFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Media_RestrictionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public Media_RestrictionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Media_Restrictions
        [HttpGet]
        public ActionResult<List<Media_Restriction>> GetMedia_Restrictions()
        {
          if (_context.Media_Restrictions == null)
          {
              return NotFound();
          }
            return _context.Media_Restrictions.ToList();
        }

        // GET: api/Media_Restrictions/5
        [HttpGet("{id}")]
        public ActionResult<List<Media_Restriction>> Media_Restrictions(int mediaId)
        {
            List<Media_Restriction> media_Restriction = _context.Media_Restrictions.Where(mr => mr.MediaId == mediaId).ToList();
            if (media_Restriction == null)
            {
                return NotFound();
            }
            return media_Restriction;
        }

        [HttpGet("{id}")]
        public ActionResult<List<Media_Restriction>> Restriction_Medias(byte restrictionId)
        {
            List<Media_Restriction> media_Restriction = _context.Media_Restrictions.Where(mr => mr.RestrictionId == restrictionId).ToList();
            if (media_Restriction == null)
            {
                return NotFound();
            }
            return media_Restriction;
        }

        // PUT: api/Media_Restrictions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public void PutMedia_Restriction(Media_Restriction media_Restriction)
        {
            _context.Entry(media_Restriction).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
            }
        }

        // POST: api/Media_Restrictions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Media_Restriction> PostMedia_Restriction(Media_Restriction media_Restriction)
        {
            _context.Media_Restrictions.Add(media_Restriction);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            { }
            return CreatedAtAction("GetMedia_Restriction", new { id = media_Restriction.MediaId }, media_Restriction);
        }

        // DELETE: api/Media_Restrictions/5
        [HttpDelete("{id}")]
        public ActionResult DeleteMedia_Restriction(int mediaId, byte restrictionId)
        {
            Media_Restriction? media_Restriction = _context.Media_Restrictions.Where(mr => mr.MediaId == mediaId).FirstOrDefault(mr => mr.RestrictionId == restrictionId);
            if (media_Restriction == null)
            {
                return NotFound();
            }
            _context.Media_Restrictions.Remove(media_Restriction);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
