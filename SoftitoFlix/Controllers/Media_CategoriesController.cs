
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult<List<Media_Category>> GetMedia_Categories()
        {
            return _context.Media_Categories.ToList();
        }

        [HttpGet("Media_Categories")]
        [Authorize]//bu media'nın tüm category'leri
        public ActionResult<List<Media_Category>> GetMedia_Category(int mediaId)
        {
            List<Media_Category>? media_Category = _context.Media_Categories.Where(mc => mc.MediaId == mediaId).ToList();
            if (media_Category == null)
            {
                return NotFound();
            }
            return media_Category;
        }

        [HttpGet("Category_Medias")]
        [Authorize]//bu category'nin medyaları
        public ActionResult<List<Media_Category>> GetCategory_Media(int categoryId)
        {
            List<Media_Category>? media_Category = _context.Media_Categories.Where(mc => mc.CategoryId == categoryId).ToList();
            if (media_Category == null)
            {
                return NotFound();
            }
            return media_Category;
        }

        // PUT: api/Media_Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
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
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult<Media_Category> PostMedia_Category(Media_Category media_Category)
        {
            _context.Media_Categories.Add(media_Category);
            try
            {
                 _context.SaveChanges();
            }
            catch (Exception)
            {
                
            }
            return CreatedAtAction("GetMedia_Category", new { id = media_Category.MediaId }, media_Category);
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
    }
}
