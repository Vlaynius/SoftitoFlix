
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
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public RolesController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        // GET: api/Roles
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult<List<ApplicationRole>> GetRoles()
        {
            return _context.Roles.ToList() ;
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<ApplicationRole> GetApplicationRole(long id)
        {
         
            ApplicationRole? applicationRole = _context.Roles.Find(id);

            if (applicationRole == null)
            {
                return NotFound();
            }

            return applicationRole;
        }

        [HttpPut("AssignRole")]
        [Authorize(Roles = "Administrator")]
        public ActionResult AssignRole(long userId, long roleId)
        {
            ApplicationUser? user = _signInManager.UserManager.FindByIdAsync(userId.ToString()).Result;
            if(user == null)
            {
                return NotFound();
            }

            ApplicationRole? role = _roleManager.Roles.FirstOrDefault(r => r.Id == roleId);
            if(role == null)
            {
                return NotFound();
            }

            _signInManager.UserManager.AddToRoleAsync(user, role.Name!).Wait();
            return Ok();
        }

        [HttpDelete("RemoveRole")]
        [Authorize(Roles = "Administrator")]
        public ActionResult RemoveRole(long userId, long roleId)
        {
            ApplicationUser? user = _signInManager.UserManager.FindByIdAsync(userId.ToString()).Result;
            if (user == null)
            {
                return NotFound();
            }

            ApplicationRole? role = _roleManager.Roles.FirstOrDefault(r => r.Id == roleId);
            if (role == null)
            {
                return NotFound();
            }

            _signInManager.UserManager.RemoveFromRoleAsync(user, role.Name!);
            return Ok();
        }

    }
}
