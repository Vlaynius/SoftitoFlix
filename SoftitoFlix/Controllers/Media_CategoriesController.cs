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
    public class Media_CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public Media_CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Media_Categories
        [HttpGet]
        [Authorize("ContentAdmin")]
        public ActionResult<List<Media_Category>> GetMedia_Categories()
        {
            return _context.Media_Categories.ToList();
        }

        // GET: api/Media_Categories/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Media_Category> GetMedia_Category(int id)
        {
            Media_Category? media_Category = _context.Media_Categories.Find(id);
            if (media_Category == null)
            {
                return NotFound();
            }
            return media_Category;
        }

        // PUT: api/Media_Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public void PutMedia_Category(Media_Category media_Category)
        {
            _context.Entry(media_Category).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
            }           
        }

        // POST: api/Media_Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Media_Category> PostMedia_Category(Media_Category media_Category)
        {
            _context.Media_Categories.Add(media_Category);
            try
            {
                 _context.SaveChanges();
            }
            catch (Exception)
            {
                if (Media_CategoryExists(media_Category.MediaId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMedia_Category", new { id = media_Category.MediaId }, media_Category);
        }

        // DELETE: api/Media_Categories/5
        [HttpDelete("{id}")]
        public ActionResult DeleteMedia_Category(int id)
        {
            if (_context.Media_Categories == null)
            {
                return NotFound();
            }
            var media_Category =  _context.Media_Categories.Find(id);
            if (media_Category == null)
            {
                return NotFound();
            }
            _context.Media_Categories.Remove(media_Category);
            _context.SaveChanges();
            return NoContent();
        }

        private bool Media_CategoryExists(int id)
        {
            return (_context.Media_Categories?.Any(e => e.MediaId == id)).GetValueOrDefault();
        }
    }
}
