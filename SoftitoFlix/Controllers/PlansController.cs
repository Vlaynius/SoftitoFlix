using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Dto.Requests.Plan;
using SoftitoFlix.Dto.Responses.Plan;
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

        // GET: api/Plans
        [HttpGet]
        public ActionResult<List<GetPlanResponse>> GetPlans()
        {
            List<Plan> plans = _context.Plans.Where(p => p.Passive == false).ToList();
            if(plans == null)
            {
                return NotFound();
            }
            List<GetPlanResponse> response = new List<GetPlanResponse>();
            foreach (Plan plan in plans)
            {
                GetPlanResponse getPlan = new GetPlanResponse();
                getPlan.Name = plan.Name;
                getPlan.Price = plan.Price;
                getPlan.Resolution = plan.Resolution;
                response.Add(getPlan);
            }
            return response;
        }

        // GET: api/Plans/5
        [HttpGet("{id}")]
        public ActionResult<GetPlanResponse> GetPlan(GetPlanIDRequest request)
        {
            Plan? plan = _context.Plans.Find(request.Id);

            if (plan == null || plan.Passive == true)
            {
                return NotFound();
            }
            GetPlanResponse response = new GetPlanResponse();
            response.Name = plan.Name;
            response.Price = plan.Price;
            response.Resolution = plan.Resolution;
            return response;
        }

        // PUT: api/Plans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult PutPlan(short id, GetPlanRequest request)
        {
            Plan? plan = _context.Plans.Find(id);
            if(plan == null)
            {
                return NotFound();
            }
            plan.Name = request.Name;
            plan.Price = request.Price;
            plan.Resolution = request.Resolution;
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
        [HttpPost("Plan")]
        [Authorize(Roles = "ContentAdmin")]
        public short PostPlan(GetPlanRequest request)
        {

            Plan plan = new Plan();
            plan.Name = request.Name;
            plan.Price = request.Price;
            plan.Resolution = request.Resolution;
            plan.Passive = false;
            _context.Plans.Add(plan);
            _context.SaveChanges();
            return plan.Id;
        }

        // DELETE: api/Plans/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeletePlan(GetPlanIDRequest request)
        {
            Plan? plan = _context.Plans.Find(request.Id);
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
        public ActionResult ChangeActivationStatus(GetPlanIDRequest request)
        {
            Plan? plan = _context.Plans.Find(request.Id);
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
