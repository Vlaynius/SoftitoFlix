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

        public struct Restriction_struct
        {
            public byte id { get; set; }
            public string name { get; set; }
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
        [HttpPut()]
        [Authorize("ContentAdmin")]
        public ActionResult PutRestriction(Restriction_struct restriction_struct)
        {
            Restriction? restriction = _context.Restrictions.Find(restriction_struct.id);
            if(restriction == null)
            {
                return NotFound();
            }

            restriction.Name = restriction_struct.name;

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
        public byte PostRestriction(string name)
        {
            Restriction restriction = new Restriction();
            restriction.Name = name;
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

        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult ChangeActivationStatus( byte id)
        {
            Restriction? restriction = _context.Restrictions.Find(id);
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
