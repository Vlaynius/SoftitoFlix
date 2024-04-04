using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Models;

namespace SoftitoFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User_PlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public User_PlansController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public struct plan_puchase
        {
            public string eMail { get; set; }
            public short planId { get; set; }
        }

        // GET: api/User_Plans
        [HttpGet("All")]
        [Authorize("Administrator")]
        public ActionResult<List<User_Plan>> GetUsers_Plans()
        {
          if (_context.User_Plans == null)
          {
              return NotFound();
          }
            return _context.User_Plans.ToList();
        }

        // GET: api/User_Plans/5
        [HttpGet]
        [Authorize]
        public ActionResult<List<User_Plan>> Users_Purchases()
        {
            long userid = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            List<User_Plan> user_Plan = _context.User_Plans.Where(p=>p.UserId == userid).ToList();
            if (user_Plan == null)
            {
                return NotFound();
            }
            return user_Plan;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<List<User_Plan>> User_Plans(long id)
        {
            List<User_Plan> user_Plan = _context.User_Plans.Where(p => p.UserId == id).ToList();
            if (user_Plan == null)
            {
                return NotFound();
            }
            return user_Plan;
        }

        // POST: api/User_Plans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public void PostUser_Plan(plan_puchase plan_Puchase)
        {
            Plan plan = _context.Plans.Find(plan_Puchase.planId)!;
            //Get Payment for plan.price
            //if(payment succesfull)
            {
                User_Plan user_Plan = new User_Plan();
                ApplicationUser applicationUser = _signInManager.UserManager.FindByEmailAsync(plan_Puchase.eMail).Result!;

                user_Plan.UserId = applicationUser.Id;
                user_Plan.PlanId = plan_Puchase.planId;
                user_Plan.StartDate = DateTime.Today;
                user_Plan.EndDate = user_Plan.StartDate.AddMonths(1);
                _context.User_Plans.Add(user_Plan);
                _context.SaveChanges();
            }
        }

    }
}
