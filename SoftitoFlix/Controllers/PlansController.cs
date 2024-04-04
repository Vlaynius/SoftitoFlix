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
    public class PlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        public struct Plan_struct
        {
            public string name { get; set; }
            public float price { get; set; }
            public string resolution { get; set; }
        }

        // GET: api/Plans
        [HttpGet]
        public ActionResult<List<Plan>> GetPlans()
        {
            return  _context.Plans.Where(p=>p.Passive == false).ToList();
        }

        // GET: api/Plans/5
        [HttpGet("{id}")]
        public ActionResult<Plan> GetPlan(short id)
        {
            Plan? plan = _context.Plans.Find(id);

            if (plan == null || plan.Passive == true)
            {
                return NotFound();
            }
            return plan;
        }

        // PUT: api/Plans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult PutPlan(short id, Plan_struct plan_struct)
        {
            Plan? plan = _context.Plans.Find(id);
            if(plan == null)
            {
                return NotFound();
            }
            plan.Name = plan_struct.name;
            plan.Price = plan_struct.price;
            plan.Resolution = plan_struct.resolution;
            _context.Entry(plan).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                
            }
            return Ok();
        }

        // POST: api/Plans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Roles = "ContentAdmin")]
        public short PostPlan(Plan_struct plan_struct)
        {
            Plan plan = new Plan();
            plan.Name = plan_struct.name;
            plan.Price = plan_struct.price;
            plan.Resolution = plan_struct.resolution;
            plan.Passive = false;
            _context.Plans.Add(plan);
            _context.SaveChanges();
            return plan.Id;
        }

        // DELETE: api/Plans/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeletePlan(short id)
        {
            Plan? plan = _context.Plans.Find(id);
            if (plan == null)
            {
                return NotFound();
            }
            plan.Passive = true;
            _context.Plans.Update(plan);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("Passive")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult ChangeActivationStatus(short id)
        {
            Plan? plan = _context.Plans.Find(id);
            if (plan == null)
            {
                return NotFound();
            }
            if (plan.Passive == true)
            {
                plan.Passive = false;
            }
            else
            {
                plan.Passive = true;
            }


            return Ok();
        }
    }
}
