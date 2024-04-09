using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Dto.Request.Directors;
using SoftitoFlix.Dto.Request.Media;
using SoftitoFlix.Dto.Response;
using SoftitoFlix.Dto.Response.Media;
using SoftitoFlix.Dto.Response.Restriction;
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
        [Authorize]
        public ActionResult<List<GetMediaResponse>> GetMedias()
        {
            List<Media> medias = _context.Medias.Where(m=>m.Passive == false).ToList();
            if(medias == null)
            {
                return NotFound();
            }
            List<GetMediaResponse> response = new List<GetMediaResponse>();
            foreach (Media media in medias)
            {
                GetMediaResponse getMedia = new GetMediaResponse();
                getMedia.Description = media.Description;
                getMedia.Name = media.Name;
                getMedia.Rating = media.Rating;
                getMedia.ReleaseDate = media.ReleaseDate;
            }
            return response;
        }

        // GET: api/Media/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<GetMediaResponse> GetMedia(MediaID_Request request)
        {
            Media? media = _context.Medias.Where(m=>m.Id == request.Id).FirstOrDefault(m => m.Passive == true);
            if (media == null)
            {
                return NotFound();
            }
            GetMediaResponse response = new GetMediaResponse();
            response.Description = media.Description;
            response.Name = media.Name;
            response.Rating = media.Rating;
            response.ReleaseDate = media.ReleaseDate;
            return response;
        }

        // GET: api/Media_Stars/5
        [HttpGet("{media_id}")]
        [Authorize]
        public ActionResult<List<Media_Star>> Media_Stars(MediaID_Request request)
        {
            List<Media_Star> media_Stars = _context.Media_Stars.Where(mr => mr.MediaId == request.Id).ToList();
            if (media_Stars == null)
            {
                return NotFound();
            }
            return media_Stars;
        }

        [HttpGet("{Director_id}")]
        [Authorize]
        public ActionResult<MediaDirectorsResponse> Media_Directors(DirectorID_Request request)
        {
            List<Media_Director>? media_Director = _context.Media_Directors.Where(md => md.DirectorId == request.Id).ToList();
            if (media_Director == null)
            {
                return NotFound();
            }
            MediaDirectorsResponse response = new MediaDirectorsResponse();
            Director director = _context.Directors.Find(request.Id)!;
            response.Director!.Id = director.Id;
            response.Director.Name = director.Name;
            foreach(Media_Director media_ in media_Director)
            {
                if(media_.Media!.Passive == false)
                {
                    GetMediaResponse getMedia = new GetMediaResponse();
                    getMedia.Description = media_.Media.Description;
                    getMedia.Name = media_.Media.Name;
                    getMedia.Rating = media_.Media.Rating;
                    getMedia.ReleaseDate = media_.Media.ReleaseDate;
                    response.Medias!.Add(getMedia);
                }
            }
            return response;
        }

        // GET: api/Media_Restrictions/5
        [HttpGet("{Media_Id}")]
        [Authorize]
        public ActionResult<MediaRestrictionResponse> Media_Restrictions(MediaID_Request request)
        {
            List<Media_Restriction> media_Restriction = _context.Media_Restrictions.Where(mr => mr.MediaId == request.Id).ToList();
            if (media_Restriction == null)
            {
                return NotFound();
            }
            MediaRestrictionResponse response = new MediaRestrictionResponse();
            Media med = _context.Medias.Find(request.Id)!;
            response.Media!.Description = med.Description;
            response.Media.Name = med.Name;
            response.Media.Rating = med.Rating;
            response.Media.ReleaseDate = med.ReleaseDate;
            foreach (Media_Restriction restriction in media_Restriction)
            {
                GetRestrictionResponse getRestriction = new GetRestrictionResponse();
                if(restriction.Restriction!.Passive == false)
                {
                    getRestriction.Id = restriction.RestrictionId;
                    getRestriction.Name = restriction.Restriction!.Name;
                }
                response.Restrictions!.Add(getRestriction);
            }
            return response;
        }

        // PUT: api/Media/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize("ContentAdmin")]
        public ActionResult PutMedia(PutMediaRequest request)
        {
            Media? media = _context.Medias.Find(request.Id);
            if(media == null)
            {
                return NotFound();
            }
            media.Name = request.Name;
            media.Description = request.Description;
            media.ReleaseDate = request.ReleaseDate;
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
        public ActionResult<GetMediaCategoriesResponse> Media_Categories(MediaID_Request request)
        {
            List<Media_Category>? media_Category = _context.Media_Categories.Where(mc => mc.MediaId == request.Id).ToList();
            if (media_Category == null)
            {
                return NotFound();
            }
            Media media = _context.Medias.Find(request.Id)!;
            GetMediaCategoriesResponse response = new GetMediaCategoriesResponse();
            response.Media!.Description = media.Description;
            response.Media.Name = media.Name;
            response.Media.Rating = media.Rating;
            response.Media.ReleaseDate = media.ReleaseDate;
            foreach(Media_Category media_ in media_Category)
            {
                GetCategoryResponse getCategory = new GetCategoryResponse();
                getCategory.Id = media_.CategoryId;
                getCategory.Name = media_.Category!.Name;
                response.Categories!.Add(getCategory);

            }

            return response;
        }

        // POST: api/Media
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize("ContentAdmin")]
        public int PostMedia(PostMediaRequest request)
        {
            Media media = new Media();
            media.Description = request.Description;
            media.Name = request.Name;
            media.Passive = false;
            media.RatedBy = 0;
            media.Rating = 0;
            media.ReleaseDate = request.ReleaseDate;
            _context.Medias.Add(media);
            _context.SaveChanges();
            return media.Id;
        }

        [HttpPost("Media_Category")]
        [Authorize("ContentAdmin")]
        public bool PostMedia_Category(PostMedia_CategoryRequest request)
        {
            Media_Category media_Category = new Media_Category();
            media_Category.CategoryId = request.CategoryId;
            media_Category.MediaId = request.MediaId;
            _context.Media_Categories.Add(media_Category);
            _context.SaveChanges();
            return true;
        }

        [HttpPost("Media_Star")]
        [Authorize("ContentAdmin")]
        public bool PostMedia_Star(PostMediaStar_Request request)
        {
            Media_Star media_Star = new Media_Star();
            media_Star.MediaId = request.MediaId;
            media_Star.StarId = request.StarId;
            _context.Media_Stars.Add(media_Star);
            _context.SaveChanges();
            return true;
        }

        [HttpPost("Media_Restriction")]
        [Authorize("ContentAdmin")]
        public bool PostMedia_Restriction(PostMediaRestriction_Request request)
        {
            Media_Restriction media_Restriction = new Media_Restriction();
            media_Restriction.MediaId = request.MediaId;
            media_Restriction.RestrictionId = request.RestrictionId;
            _context.Media_Restrictions.Add(media_Restriction);
            _context.SaveChanges();
            return true;
        }

        [HttpPost("Media_Director")]
        [Authorize("ContentAdmin")]
        public bool PostMedia_Director(PostMediaDirector_Request request)
        {
            Media_Director media_Director = new Media_Director();
            media_Director.MediaId = request.MediaId;
            media_Director.DirectorId = request.DirectorId;
            _context.Media_Directors.Add(media_Director);
            _context.SaveChanges();
            return true;
        }

        // DELETE: api/Media/5
        [HttpDelete("Media_id")]
        [Authorize("ContentAdmin")]
        public ActionResult DeleteMedia(MediaID_Request request)
        {
            Media? media = _context.Medias.Find(request.Id);
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
        public ActionResult DeleteMedia_Category(PostMedia_CategoryRequest request)
        {
            Media_Category? media_Category = _context.Media_Categories.Where(mc => mc.MediaId == request.MediaId).FirstOrDefault(mc => mc.CategoryId == request.CategoryId);
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
        public ActionResult DeleteMedia_Director(PostMediaDirector_Request request)
        {
            Media_Director? media_Director = _context.Media_Directors.Where(md => md.MediaId == request.MediaId).FirstOrDefault(md => md.DirectorId == request.DirectorId);
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
        public ActionResult DeleteMedia_Restriction(PostMediaRestriction_Request request)
        {
            Media_Restriction? media_Restriction = _context.Media_Restrictions.Where(mr => mr.MediaId == request.MediaId).FirstOrDefault(mr => mr.RestrictionId == request.RestrictionId);
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
        public ActionResult DeleteMedia_Star(PostMediaStar_Request request)
        {
            Media_Star? media_Star = _context.Media_Stars.Where(mr => mr.MediaId == request.MediaId).FirstOrDefault(mr => mr.StarId == request.StarId);
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
        public ActionResult RateMedia(RateMediaRequest request)// [0,10]
        {
            Media? media = _context.Medias.Find(request.MediaId);
            if(media == null)
            {
                return NotFound();
            }
            float totalPoint = media.RatedBy * media.Rating;
            totalPoint += request.Rating;
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
