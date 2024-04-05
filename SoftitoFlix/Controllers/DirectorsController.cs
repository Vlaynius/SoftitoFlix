using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Dto.Request.Directors;
using SoftitoFlix.Dto.Response;
using SoftitoFlix.Dto.Response.Directors;
using SoftitoFlix.Models;

namespace SoftitoFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DirectorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Directors
        [HttpGet]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult<List<Director>> GetDirectors()
        {
            return _context.Directors.ToList();
        }

        // GET: api/Directors/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<GetDirectorResponse> GetDirector(DirectorID_Request request)
        {
            Director? director = _context.Directors.Find(request.Id);
            if (director == null)
            {
                return NotFound();
            }
            GetDirectorResponse response = new GetDirectorResponse();
            response.Id = director.Id;
            response.Name = director.Name;
            return response;
        }


        [HttpGet("{Media_id}")]
        [Authorize]
        public ActionResult<GetDirectorMediasResponse> Director_Medias(DirectorID_Request request)
        {
            List<Media_Director>? media_Director = _context.Media_Directors.Where(md => md.DirectorId == request.Id).ToList();
            if (media_Director == null)
            {
                return NotFound();
            }
            GetDirectorMediasResponse response = new GetDirectorMediasResponse();
            Director? director = _context.Directors.Find(request.DirectorId);
            GetDirectorResponse director1 = new GetDirectorResponse();
            director1.Id = director1.Id;
            director1.Name = director!.Name;
            response.Director = director1;
            foreach (Media_Director media_ in media_Director)
            {
                GetMediaResponse getMedia = new GetMediaResponse();
                getMedia.Description = media_.Media!.Description;
                getMedia.Name = media_.Media.Name;
                getMedia.Rating = media_.Media.Rating;
                getMedia.ReleaseDate = media_.Media.ReleaseDate;
                response.Medias!.Add(getMedia);
            }
            return response;
        }

        // PUT: api/Directors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult PutDirector(PutDirectorRequest request)
        {
            Director? director = _context.Directors.Find(request.Id);
            if (director == null)
            {
                return NotFound();
            }
            director.Name = request.Name;
            _context.Update(director);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            { }
            return Ok();
        }

        // POST: api/Directors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "ContentAdmin")]
        public int PostDirector(PostDirectorRequest request)
        {
            Director director = new Director();
            director.Name = request.Name;
            _context.Directors.Add(director);
            _context.SaveChanges();
            return director.Id;
        }

        [HttpDelete]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeleteDirector(DirectorID_Request request)
        {
            Director? director = _context.Directors.Find(request.Id);
            if(director == null)
            {
                return NotFound();
            }

            _context.Directors.Remove(director);
            _context.SaveChanges();
            return Ok();
        }
    }
}
