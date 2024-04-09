using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Dto.Requests.Star;
using SoftitoFlix.Dto.Response;
using SoftitoFlix.Dto.Responses.Star;
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
        [Authorize]
        public ActionResult<GetStarResponse> GetStar(StarID_Request request)
        {
            Star? star = _context.Stars.Find(request.Id);
            if (star == null)
            {
                return NotFound();
            }
            GetStarResponse response = new GetStarResponse();
            response.Id = star.Id;
            response.Name = star.Name;
            return response;
        }

        [HttpGet("{star_id}")]
        [Authorize]
        public ActionResult<GetStarsMediaResponse> Stars_Media(StarID_Request request)
        {
            List<Media_Star> Stars_Media = _context.Media_Stars.Where(mr => mr.StarId == request.Id).ToList();
            if (Stars_Media == null)
            {
                return NotFound();
            }
            GetStarsMediaResponse response = new GetStarsMediaResponse();
            Star star = _context.Stars.Find(request.Id)!;

            response.Star.Id = star.Id;
            response.Star.Name = star.Name;
            foreach (Media_Star item in Stars_Media)
            {
                response.MediaIDs.Add(item.MediaId);
            }
            return response;
        }

        // PUT: api/Stars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize("ContentAdmin")]
        public ActionResult PutStar(PutStarRequest request)
        {
            Star? star = _context.Stars.Find(request.Id);
            if(star == null)
            {
                return NotFound();
            }
            star.Name = request.Name;
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
        public int PostStar(PostStarRequest request)
        {
            Star star = new Star();
            star.Name = request.Name;
            _context.Stars.Add(star);
            _context.SaveChanges();

            return star.Id;
        }

        // DELETE: api/Media_Stars/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeleteMedia_Star(GetMediaStarRequest request)
        {
            Media_Star? media_Star =  _context.Media_Stars.Where(ms=>ms.MediaId == request.mediaId).FirstOrDefault(ms=>ms.StarId == request.starId);
            if (media_Star == null)
            {
                return NotFound();
            }
            _context.Media_Stars.Remove(media_Star);
            _context.SaveChanges();
            return Ok();
        }

    }
}
