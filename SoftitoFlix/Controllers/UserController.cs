using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Models;

namespace SoftitoFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserController(/*ApplicationDbContext context,*/ SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            //_context = context;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: api/User
        [HttpGet]
        public  ActionResult<List<ApplicationUser>> GetUsers()
        {
          if (_signInManager.UserManager.Users == null)
          {
              return NotFound();
          }
            return  _signInManager.UserManager.Users.ToList();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public  ActionResult<ApplicationUser> GetApplicationUser(long id)
        {
          if (_signInManager.UserManager.Users == null)
          {
              return NotFound();
          }
          ApplicationUser? applicationUser =  _signInManager.UserManager.Users.Where(r=>r.Id == id).FirstOrDefault();
          if (applicationUser == null)
          {
              return NotFound();
          }
          return applicationUser;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult PutApplicationUser( ApplicationUser applicationUser)
        {
            ApplicationUser? existingUser = _signInManager.UserManager.FindByIdAsync(applicationUser.Id.ToString()).Result;
            if(existingUser == null)
            {
                return NotFound();
            }
            existingUser.Name = applicationUser.Name;
            existingUser.BirthDate = applicationUser.BirthDate;
            existingUser.Email = applicationUser.Email;
            existingUser.Passive = applicationUser.Passive;
            existingUser.PhoneNumber = applicationUser.PhoneNumber;
            existingUser.UserName = applicationUser.UserName;
            _signInManager.UserManager.UpdateAsync(existingUser);
            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public long PostApplicationUser(ApplicationUser applicationUser, string Password)
        {
            _signInManager.UserManager.CreateAsync(applicationUser, Password).Wait();
            return applicationUser.Id;
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public ActionResult DeleteApplicationUser(long id)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByIdAsync(id.ToString()).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            applicationUser.Passive = true;
            _signInManager.UserManager.UpdateAsync(applicationUser).Wait();
            return NoContent();
        }

    }
}
