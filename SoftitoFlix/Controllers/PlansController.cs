using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public void PutPlan( Plan plan)
        {
            _context.Entry(plan).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                
            }
        }

        // POST: api/Plans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public short PostPlan(Plan plan)
        {
            _context.Plans.Add(plan);
            _context.SaveChanges();
            return plan.Id;
        }

        // DELETE: api/Plans/5
        [HttpDelete("{id}")]
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

    }
}
