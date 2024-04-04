using Microsoft.AspNetCore.Authorization;
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

        public struct Media_struct
        {
            public string name { get; set; }
            public string? Description { get; set; }
            public short ReleaseDate { get; set; }
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
        [HttpGet("{media_id}")]
        [Authorize]
        public ActionResult<List<Media_Star>> Media_Stars(int media_id)
        {
            List<Media_Star> media_Stars = _context.Media_Stars.Where(mr => mr.MediaId == media_id).ToList();
            if (media_Stars == null)
            {
                return NotFound();
            }
            return media_Stars;
        }

        [HttpGet("{Director_id}")]
        [Authorize]
        public ActionResult<List<Media_Director>> Media_Directors(int Director_id)
        {
            List<Media_Director>? media_Director = _context.Media_Directors.Where(md => md.DirectorId == Director_id).ToList();
            if (media_Director == null)
            {
                return NotFound();
            }
            return media_Director;
        }

        // GET: api/Media_Restrictions/5
        [HttpGet("{Media_Id}")]
        [Authorize]
        public ActionResult<List<Media_Restriction>> Media_Restrictions(int Media_Id)
        {
            List<Media_Restriction> media_Restriction = _context.Media_Restrictions.Where(mr => mr.MediaId == Media_Id).ToList();
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
        public ActionResult PutMedia( int id, Media_struct media_struct)
        {
            Media? media = _context.Medias.Find(id);
            if(media == null)
            {
                return NotFound();
            }
            media.Name = media_struct.name;
            media.Description = media_struct.Description;
            media.ReleaseDate = media_struct.ReleaseDate;
            _context.Medias.Update(media);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            { }
            return Ok();
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
        public int PostMedia(Media_struct media_struct)
        {
            Media media = new Media();
            media.Description = media_struct.Description;
            media.Name = media_struct.name;
            media.Passive = false;
            media.RatedBy = 0;
            media.Rating = 0;
            media.ReleaseDate = media_struct.ReleaseDate;
            _context.Medias.Add(media);
            _context.SaveChanges();
            return media.Id;
        }

        [HttpPost("Media_Category")]
        [Authorize("ContentAdmin")]
        public bool PostMedia_Category(int mediaId, int categoryId)
        {
            Media_Category media_Category = new Media_Category();
            media_Category.CategoryId = categoryId;
            media_Category.MediaId = mediaId;
            _context.Media_Categories.Add(media_Category);
            _context.SaveChanges();
            return true;
        }

        [HttpPost("Media_Star")]
        [Authorize("ContentAdmin")]
        public bool PostMedia_Star(int mediaId, int starId)
        {
            Media_Star media_Star = new Media_Star();
            media_Star.MediaId = mediaId;
            media_Star.StarId = starId;
            _context.Media_Stars.Add(media_Star);
            _context.SaveChanges();
            return true;
        }

        [HttpPost("Media_Restriction")]
        //[Authorize("ContentAdmin")]
        public bool PostMedia_Restriction(int mediaId, byte restrictionId)
        {
            Media_Restriction media_Restriction = new Media_Restriction();
            media_Restriction.MediaId = mediaId;
            media_Restriction.RestrictionId = restrictionId;
            _context.Media_Restrictions.Add(media_Restriction);
            _context.SaveChanges();
            return true;
        }

        [HttpPost("Media_Director")]
        //[Authorize("ContentAdmin")]
        public bool PostMedia_Director(int mediaId, int directorId)
        {
            Media_Director media_Director = new Media_Director();
            media_Director.MediaId = mediaId;
            media_Director.DirectorId = directorId;
            _context.Media_Directors.Add(media_Director);
            _context.SaveChanges();
            return true;
        }

        // DELETE: api/Media/5
        [HttpDelete("Media_id")]
        [Authorize("ContentAdmin")]
        public ActionResult DeleteMedia(int Media_id)
        {
            Media? media = _context.Medias.Find(Media_id);
            if (media == null)
            {
                return NotFound();
            }
            media.Passive = false;
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Media_Categories/5
        [HttpDelete("Media_Category")]
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
        [HttpDelete("Media_Director")]
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

        [HttpDelete("Media_Restriction")]
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
        [HttpDelete("Media_Star")]
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

        [HttpPut("Rate")]
        [Authorize]
        public ActionResult RateMedia(int media_id, byte Rating)// [0,10]
        {
            Media? media = _context.Medias.Find(media_id);
            if(media == null)
            {
                return NotFound();
            }
            float totalPoint = media.RatedBy * media.Rating;
            totalPoint += Rating;
            media.RatedBy++;
            media.Rating = totalPoint / media.RatedBy;

            _context.Medias.Update(media);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            { }
            return Ok();
        }

    }
}
