using System;
using System.Collections.Generic;
using System.Data;
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
    public class DirectorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DirectorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Directors
        [HttpGet]
        [Authorize]
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

        // PUT: api/Directors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public void PutDirector(Director director)
        {
            _context.Entry(director).State = EntityState.Modified;

            try
            {
                 _context.SaveChanges();
            }
            catch (Exception)
            { }
        }

        // POST: api/Directors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "ContentAdmin")]
        public int PostDirector(Director director)
        {
            _context.Directors.Add(director);
            _context.SaveChanges();
            return director.Id;
        }

    }
}