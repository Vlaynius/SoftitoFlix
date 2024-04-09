using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftitoFlix.Data;
using SoftitoFlix.Models;
using SoftitoFlix.Dto.Response;
using SoftitoFlix.Dto.Request;

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
        [Authorize]
        public ActionResult<List<GetCategoryResponse>> GetCategories()
        {
            List<Category> categories= _context.Categories.ToList();
            if(categories == null)
            {
                return NotFound();
            }
            List<GetCategoryResponse> response = new List<GetCategoryResponse>();
            foreach (Category category in categories)
            {
                GetCategoryResponse getCategory = new GetCategoryResponse();
                getCategory.Id = category.Id;
                getCategory.Name = category.Name;
                response.Add(getCategory);
            }
            return response;
        }

        // GET: api/Categories/5 *****
        [HttpGet("{id}")]
        [Authorize]
        public  ActionResult<GetCategoryResponse> GetCategory(GetCategoryRequest request)
        {
            Category? category = _context.Categories.Where(c => c.Id == request.Id).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }
            GetCategoryResponse response = new GetCategoryResponse();
            response.Id = category.Id;
            response.Name = category.Name;
            return response;
        }

        [HttpGet("{CategoryId}")]
        [Authorize]
        public ActionResult<GetCategoryMediasResponse> Category_Medias(GetCategoryRequest request)
        {
            
            GetCategoryMediasResponse response = new GetCategoryMediasResponse();
            response.Category = _context.Categories.Find(request.Id);
            if(response.Category == null)
            {
                return NotFound();
            }
            List<Media_Category>? media_Category = _context.Media_Categories.Where(mc => mc.CategoryId == request.Id).ToList();
            foreach(Media_Category media_c in media_Category)
            {
                GetMediaResponse getMediaResponse = new GetMediaResponse();
                getMediaResponse.Description = media_c.Media.Description;
                getMediaResponse.Name = media_c.Media.Name;
                getMediaResponse.Rating = media_c.Media.Rating;
                getMediaResponse.ReleaseDate = media_c.Media.ReleaseDate;
                response.medias!.Add(getMediaResponse);
            }
            return response;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult PutCategory(PutCategoryRequest request)
        {
            Category? category = _context.Categories.Find(request.Id);
            if(category == null)
            {
                return NotFound();
            }
            category.Name = request.Name;
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
        public int PostCategory(PostCategoryRequest request)
        {
            Category category = new Category();
            category.Name = request.Name;
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category.Id;
        }

    }
}
