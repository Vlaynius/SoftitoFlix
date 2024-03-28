using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
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
        public ActionResult<List<Restriction>> GetRestrictions()
        {
            return _context.Restrictions.Where(p => p.Passive == false).ToList();
        }

        // GET: api/Restrictions/5
        [HttpGet("{id}")]
        public ActionResult<Restriction> GetRestriction(byte id)
        {
            Restriction? restriction = _context.Restrictions.Find(id);

            if (restriction == null || restriction.Passive == true)
            {
                return NotFound();
            }
            return restriction;
        }

        [HttpGet("{id}")]
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
        public void PutRestriction( Restriction restriction)
        {
            _context.Entry(restriction).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                
            }
        }

        // POST: api/Restrictions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public byte PostRestriction(Restriction restriction)
        {
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
        public ActionResult DeleteRestriction(byte id)
        { 
            Restriction? restriction = _context.Restrictions.Find(id);
            if (restriction == null)
            {
                return NotFound();
            }
            restriction.Passive = true;
            _context.Restrictions.Update(restriction);
            _context.SaveChanges();

            return NoContent();
        }
        
    }
}
