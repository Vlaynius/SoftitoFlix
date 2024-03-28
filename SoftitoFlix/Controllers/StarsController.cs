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

    }
}
