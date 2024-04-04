using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftitoFlix.Data;
using SoftitoFlix.Models;

namespace SoftitoFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        //[Authorize]
        public ActionResult<List<Category>> GetCategories()
        {
            return  _context.Categories.ToList();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        //[Authorize]
        public  ActionResult<Category> GetCategory(int id)
        {
            Category? category =  _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }

        [HttpGet("{CategoryId}")]
        [Authorize]//bu category'nin medyaları
        public ActionResult<List<Media_Category>> Category_Medias(int categoryId)
        {
            List<Media_Category>? media_Category = _context.Media_Categories.Where(mc => mc.CategoryId == categoryId).ToList();
            if (media_Category == null)
            {
                return NotFound();
            }
            return media_Category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult PutCategory( int id,  string name)
        {
            Category? category = _context.Categories.Find(id);
            if(category == null)
            {
                return NotFound();
            }
            category.Name = name;
            _context.Categories.Update(category);
            try
            {
                 _context.SaveChanges();
            }
            catch (Exception)
            { }
            return Ok();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "ContentAdmin")]
        public int PostCategory(string name)
        {
            Category category = new Category();
            category.Name = name;
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category.Id;
        }

    }
}
