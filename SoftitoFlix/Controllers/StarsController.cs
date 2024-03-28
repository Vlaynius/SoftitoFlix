using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Models;

namespace SoftitoFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StarsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Stars
        [HttpGet]
        [Authorize]
        public ActionResult<List<Star>> GetStars()
        {
            return _context.Stars.ToList();
        }

        // GET: api/Stars/5
        [HttpGet("{id}")]
        public ActionResult<Star> GetStar(int id)
        {
            Star? star = _context.Stars.Find(id);
            if (star == null)
            {
                return NotFound();
            }
            return star;
        }

        [HttpGet("{star_id}")]
        [Authorize]
        public ActionResult<List<Media_Star>> Stars_Media(int starId)
        {
            List<Media_Star> Stars_Media = _context.Media_Stars.Where(mr => mr.StarId == starId).ToList();
            if (Stars_Media == null)
            {
                return NotFound();
            }
            return Stars_Media;
        }

        // PUT: api/Stars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize("ContentAdmin")]
        public void PutStar( Star star)
        {
            _context.Entry(star).State = EntityState.Modified;
            try
            {
                 _context.SaveChanges();
            }
            catch (Exception)
            {
                
            }           
        }

        // POST: api/Stars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize("ContentAdmin")]
        public int PostStar(Star star)
        {
            _context.Stars.Add(star);
            _context.SaveChanges();

            return star.Id;
        }

        // DELETE: api/Media_Stars/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeleteMedia_Star(int mediaId, int starId)
        {
            Media_Star? media_Star =  _context.Media_Stars.Where(ms=>ms.MediaId == mediaId).FirstOrDefault(ms=>ms.StarId == starId);
            if (media_Star == null)
            {
                return NotFound();
            }
            _context.Media_Stars.Remove(media_Star);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
