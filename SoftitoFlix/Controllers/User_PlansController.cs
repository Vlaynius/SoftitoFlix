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
        public ActionResult<List<User_Plan>> User_Plans(long userid)
        {
            List<User_Plan> user_Plan = _context.User_Plans.Where(p => p.UserId == userid).ToList();
            if (user_Plan == null)
            {
                return NotFound();
            }
            return user_Plan;
        }

        // PUT: api/User_Plans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public void PutUser_Plan(User_Plan user_Plan)
        {
            _context.Entry(user_Plan).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception){ }
        }

        // POST: api/User_Plans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public void PostUser_Plan(string eMail, short planId)
        {
            Plan plan = _context.Plans.Find(planId)!;
            //Get Payment fpr plan.price
            //if(payment succesfull)
            {
                User_Plan user_Plan = new User_Plan();
                ApplicationUser applicationUser = _signInManager.UserManager.FindByEmailAsync(eMail).Result!;

                user_Plan.UserId = applicationUser.Id;
                user_Plan.PlanId = planId;
                _context.User_Plans.Add(user_Plan);
                _context.SaveChanges();
            }
        }

    }
}
