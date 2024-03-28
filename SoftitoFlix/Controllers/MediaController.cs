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
    public class MediaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MediaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Media
        [HttpGet]
        [Authorize("ContentAdmin")]
        public ActionResult<List<Media>> GetMedias()
        {
            return _context.Medias.ToList();
        }

        // GET: api/Media/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Media> GetMedia(int id)
        {
            Media? media = _context.Medias.Where(m=>m.Id == id).FirstOrDefault(m => m.Passive == true);
            if (media == null)
            {
                return NotFound();
            }
            return media;
        }

        // GET: api/Media_Stars/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<List<Media_Star>> Media_Stars(int mediaId)
        {
            List<Media_Star> media_Stars = _context.Media_Stars.Where(mr => mr.MediaId == mediaId).ToList();
            if (media_Stars == null)
            {
                return NotFound();
            }
            return media_Stars;
        }

        [HttpGet("{Director_id}")]
        [Authorize]
        public ActionResult<List<Media_Director>> Media_Directors(int directorId)
        {
            List<Media_Director>? media_Director = _context.Media_Directors.Where(md => md.DirectorId == directorId).ToList();
            if (media_Director == null)
            {
                return NotFound();
            }
            return media_Director;
        }

        // GET: api/Media_Restrictions/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<List<Media_Restriction>> Media_Restrictions(int mediaId)
        {
            List<Media_Restriction> media_Restriction = _context.Media_Restrictions.Where(mr => mr.MediaId == mediaId).ToList();
            if (media_Restriction == null)
            {
                return NotFound();
            }
            return media_Restriction;
        }

        // PUT: api/Media/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize("ContentAdmin")]
        public void PutMedia( Media media)
        {
            _context.Entry(media).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            { }
        }

        [HttpGet("Media_Categories")]
        [Authorize]
        public ActionResult<List<Media_Category>> Media_Categories(int mediaId)
        {
            List<Media_Category>? media_Category = _context.Media_Categories.Where(mc => mc.MediaId == mediaId).ToList();
            if (media_Category == null)
            {
                return NotFound();
            }
            return media_Category;
        }

        // POST: api/Media
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize("ContentAdmin")]
        public int PostMedia(Media media)
        {
            _context.Medias.Add(media);
            _context.SaveChanges();
            return media.Id;
        }

        // DELETE: api/Media/5
        [HttpDelete("{id}")]
        [Authorize("ContentAdmin")]
        public ActionResult DeleteMedia(int id)
        {
            Media? media = _context.Medias.Find(id);
            if (media == null)
            {
                return NotFound();
            }
            media.Passive = false;
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Media_Categories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeleteMedia_Category(int mediaId, int categoryId)
        {
            Media_Category? media_Category = _context.Media_Categories.Where(mc => mc.MediaId == mediaId).FirstOrDefault(mc => mc.CategoryId == categoryId);
            if (media_Category == null)
            {
                return NotFound();
            }
            _context.Media_Categories.Remove(media_Category);
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Media_Directors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeleteMedia_Director(int mediaId, int directorId)
        {
            Media_Director? media_Director = _context.Media_Directors.Where(md => md.MediaId == mediaId).FirstOrDefault(md => md.DirectorId == directorId);
            if (media_Director == null)
            {
                return NotFound();
            }
            _context.Media_Directors.Remove(media_Director);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
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

        // DELETE: api/Media_Stars/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeleteMedia_Star(int mediaId, int starId)
        {
            Media_Star? media_Star = _context.Media_Stars.Where(mr => mr.MediaId == mediaId).FirstOrDefault(mr => mr.StarId == starId);
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
