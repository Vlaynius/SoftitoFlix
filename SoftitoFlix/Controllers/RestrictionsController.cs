using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Dto.Requests;
using SoftitoFlix.Dto.Requests.Plan;
using SoftitoFlix.Dto.Requests.Restriction;
using SoftitoFlix.Dto.Response.Restriction;
using SoftitoFlix.Models;

namespace SoftitoFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestrictionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RestrictionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Restrictions
        [HttpGet]
        [Authorize]
        public ActionResult<List<GetRestrictionResponse>> GetRestrictions()
        {
            List<Restriction> restrictions = _context.Restrictions.Where(p => p.Passive == false).ToList();
            if(restrictions == null)
            {
                return NotFound();
            }
            List<GetRestrictionResponse> response = new List<GetRestrictionResponse>();
            foreach(Restriction restriction in restrictions)
            {
                GetRestrictionResponse getRestriction = new GetRestrictionResponse();
                getRestriction.Id = restriction.Id;
                getRestriction.Name = restriction.Name;
                response.Add(getRestriction);
            }

            return response;
        }

        // GET: api/Restrictions/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<GetRestrictionResponse> GetRestriction(GetRestrictionRequest request)
        {
            Restriction? restriction = _context.Restrictions.Find(request.Id);

            if (restriction == null || restriction.Passive == true)
            {
                return NotFound();
            }

            GetRestrictionResponse response = new GetRestrictionResponse();
            response.Id = restriction.Id;
            response.Name = restriction.Name;
            return response;
        }

        [HttpGet("restriction_id")]
        [Authorize]
        public ActionResult<List<Media_Restriction>> Restriction_Medias(byte restrictionId)
        {
            List<Media_Restriction> media_Restriction = _context.Media_Restrictions.Where(mr => mr.RestrictionId == restrictionId).ToList();
            if (media_Restriction == null)
            {
                return NotFound();
            }
            return media_Restriction;
        }

        // PUT: api/Restrictions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize("ContentAdmin")]
        public ActionResult PutRestriction(GetRestrictionRequest request)
        {
            Restriction? restriction = _context.Restrictions.Find(request.Id);
            if(restriction == null)
            {
                return NotFound();
            }

            restriction.Name = request.Name;

            _context.Entry(restriction).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                
            }
            return Ok();
        }

        // POST: api/Restrictions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "ContentAdmin")]
        public byte PostRestriction(PostRestrictionRequest request)
        {
            Restriction restriction = new Restriction();
            restriction.Name = request.Name;
            restriction.Passive = false;
            _context.Restrictions.Add(restriction);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
               
            }
            return restriction.Id;
        }

        // DELETE: api/Restrictions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeleteRestriction(RestrictionIDRequest request)
        { 
            Restriction? restriction = _context.Restrictions.Find(request.Id);
            if (restriction == null)
            {
                return NotFound();
            }
            restriction.Passive = true;
            _context.Restrictions.Update(restriction);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut()]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult ChangeActivationStatus(RestrictionIDRequest request)
        {
            Restriction? restriction = _context.Restrictions.Find(request.Id);
            if(restriction == null)
            {
                return NotFound();
            }
            if(restriction.Passive == true)
            {
                restriction.Passive = false;
            }
            else
            {
                restriction.Passive = true;
            }


            return Ok();
        }

    }
}
