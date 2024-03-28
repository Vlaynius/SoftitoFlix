
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Models;

namespace SoftitoFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Media_DirectorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public Media_DirectorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Media_Directors
        [HttpGet]
        public ActionResult<List<Media_Director>> GetMedia_Directors()
        {
            return _context.Media_Directors.ToList();
        }

        // GET: api/Media_Directors/5
        [HttpGet("{Director_id}")]
        public ActionResult<List<Media_Director>> Media_Directors(int directorId)
        {
            List<Media_Director>? media_Director = _context.Media_Directors.Where(md=>md.DirectorId == directorId).ToList();
            if (media_Director == null)
            {
                return NotFound();
            }
            return media_Director;
        }

        [HttpGet("{Media_id}")]
        public ActionResult<List<Media_Director>> Directors_Medias(int mediaId)
        {
            List<Media_Director>? media_Director = _context.Media_Directors.Where(md => md.MediaId == mediaId).ToList();
            if (media_Director == null)
            {
                return NotFound();
            }
            return media_Director;
        }

        // PUT: api/Media_Directors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public void PutMedia_Director(Media_Director media_Director)
        {
            _context.Entry(media_Director).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            { }
        }

        // POST: api/Media_Directors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Media_Director> PostMedia_Director(Media_Director media_Director)
        {
            _context.Media_Directors.Add(media_Director);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            { }
            return CreatedAtAction("GetMedia_Director", new { id = media_Director.MediaId }, media_Director);
        }

        // DELETE: api/Media_Directors/5
        [HttpDelete("{id}")]
        public ActionResult DeleteMedia_Director(int mediaId, int directorId)
        {
            Media_Director? media_Director = _context.Media_Directors.Where(md=>md.MediaId == mediaId).FirstOrDefault(md=>md.DirectorId == directorId);
            if (media_Director == null)
            {
                return NotFound();
            }
            _context.Media_Directors.Remove(media_Director);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
