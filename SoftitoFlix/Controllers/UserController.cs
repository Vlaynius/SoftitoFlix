using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Models;
using System.Security.Claims;

namespace SoftitoFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController( SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // GET: api/User
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public  ActionResult<List<ApplicationUser>> GetUsers()
        {
            if (_signInManager.UserManager.Users == null)
            {
                return NotFound();
            }
            return  _signInManager.UserManager.Users.AsNoTracking().ToList();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        [Authorize]
        public  ActionResult<ApplicationUser> GetApplicationUser(long id)
        {
            if(User.IsInRole("Administrator") == false)
            {
                if(User.FindFirstValue(ClaimTypes.NameIdentifier) == id.ToString() == false)
                {
                    return Unauthorized();
                }
            }
            if (_signInManager.UserManager.Users == null)
            {
                return NotFound();
            }
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByIdAsync(id.ToString()).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            return applicationUser;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult PutApplicationUser( ApplicationUser applicationUser)
        {
            ApplicationUser? user = _signInManager.UserManager.Users.Where(u => u.Id == applicationUser.Id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            user.Name = applicationUser.Name;
            user.BirthDate = applicationUser.BirthDate;
            user.Email = applicationUser.Email;
            user.PhoneNumber = applicationUser.PhoneNumber;
            user.UserName = applicationUser.UserName;
            _signInManager.UserManager.UpdateAsync(user).Wait();
            return Ok();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<object> PostApplicationUser(ApplicationUser applicationUser)
        {
            if(User.Identity.IsAuthenticated == true)
            {
                return BadRequest();
            }
            IdentityResult IdentityResult =_signInManager.UserManager.CreateAsync(applicationUser,applicationUser.Password).Result;
            if(IdentityResult != IdentityResult.Success)
            {
                return IdentityResult.Errors.FirstOrDefault()!.Description;
            }
            return applicationUser.Id;
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public ActionResult PassifyApplicationUser(long id)
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

        [HttpPut("Activate")]
        public ActionResult ActivateApplicationUser(long id)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByIdAsync(id.ToString()).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            applicationUser.Passive = false;
            _signInManager.UserManager.UpdateAsync(applicationUser).Wait();
            return NoContent();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("SetPassword")]
        public ActionResult<string> SetPassword(string userName, string NewPassword)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return Problem();//Kullanıcı Yok
            }
            try
            {
                _signInManager.UserManager.RemovePasswordAsync(applicationUser).Wait();
                _signInManager.UserManager.AddPasswordAsync(applicationUser, NewPassword).Wait();
            }
            catch (Exception)
            {
                return Ok("An Error Accured");
            }
            return Ok("Password Assigned Successfully");
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<bool> ChangePassword(string userName, string currentPassword, string NewPassword)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return false;//Kullanıcı Yok
            }

            var changePasswordResult = await _signInManager.UserManager.ChangePasswordAsync(applicationUser, currentPassword, NewPassword);
            return changePasswordResult.Succeeded;
        }

        [HttpPost("PasswordReset")]
        public string? PasswordReset(string userName)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return null;//Kullanıcı Yok
            }
            return _signInManager.UserManager.GeneratePasswordResetTokenAsync(applicationUser).Result;
        }

        [HttpPost("ValidateResetPassword")]
        public ActionResult<string> ValidateResetPassword(string UserName, string token, string newPassword)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(UserName).Result;
            if (applicationUser == null)
            {
                return NotFound();//Kullanıcı Yok
            }
            IdentityResult identityResult = _signInManager.UserManager.ResetPasswordAsync(applicationUser, token, newPassword).Result;
            if (!identityResult.Succeeded)
            {
                return identityResult.Errors.First().Description;
            }
            return Ok("Password Reset Successfull");
        }

    }
}
