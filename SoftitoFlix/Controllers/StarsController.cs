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

        public struct Star_struct
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public struct Media_star
        {
            public int mediaId { get; set; }
            public int starId { get; set; }
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
        public ActionResult<List<Media_Star>> Stars_Media(int star_id)
        {
            List<Media_Star> Stars_Media = _context.Media_Stars.Where(mr => mr.StarId == star_id).ToList();
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
        public ActionResult PutStar(Star_struct struct_star)
        {
            Star? star = _context.Stars.Find(struct_star.id);
            if(star == null)
            {
                return NotFound();
            }
            star.Name = struct_star.name;
            _context.Stars.Update(star);
            try
            {
                 _context.SaveChanges();
            }
            catch (Exception)
            {
                
            }
            return Ok();
        }

        // POST: api/Stars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize("ContentAdmin")]
        public int PostStar(string name)
        {
            Star star = new Star();
            star.Name = name;
            _context.Stars.Add(star);
            _context.SaveChanges();

            return star.Id;
        }

        // DELETE: api/Media_Stars/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeleteMedia_Star(Media_star mediaStar)
        {
            Media_Star? media_Star =  _context.Media_Stars.Where(ms=>ms.MediaId == mediaStar.mediaId).FirstOrDefault(ms=>ms.StarId == mediaStar.starId);
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
