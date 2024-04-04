using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
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
            return  _context.Directors.ToList();
        }

        // GET: api/Directors/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Director> GetDirector(int id)
        {
            Director? director =  _context.Directors.Find(id);
            if (director == null)
            {
                return NotFound();
            }
            return director;
        }


        [HttpGet("{Media_id}")]
        [Authorize]
        public ActionResult<List<Media_Director>> Directors_Medias(int mediaId)
        {
            List<Media_Director>? media_Director = _context.Media_Directors.Where(md => md.MediaId == mediaId).ToList();
            if (media_Director == null)
            {
                return NotFound();
            }
            return media_Director;
        }

        // PUT: api/Directors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult PutDirector(int id,  string name)
        {
            Director? director = _context.Directors.Find(id);
            if(director == null)
            {
                return NotFound();
            }
            director.Name = name;
            _context.Directors.Update(director);
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
        public int PostDirector(string name)
        {
            Director director = new Director();
            director.Name = name;
            _context.Directors.Add(director);
            _context.SaveChanges();
            return director.Id;
        }

    }
}
