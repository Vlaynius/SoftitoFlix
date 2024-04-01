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
        
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;


        public UserController( SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        public struct LogInModel
        {
            public string userName { get; set; }
            public string password { get; set; }
        }

        // GET: api/User
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public  ActionResult<List<ApplicationUser>> GetUsers(bool IncludePassiveUsers = true, bool IncludeDeletedUsers = true)
        {
            IQueryable<ApplicationUser> users = _signInManager.UserManager.Users;
            if (IncludePassiveUsers == false)
            {
                users = users.Where(u=>u.Passive == false);
            }
            if(IncludeDeletedUsers == false)
            {
                users = users.Where(u => u.Deleted == false);
            }
            return users.AsNoTracking().ToList();
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
            if (User.IsInRole("CustomerRepresentative") == false)
            {
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == applicationUser.Id.ToString() == false)
                {
                    return Unauthorized();
                }
            }
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
        [Authorize]
        public ActionResult DeleteApplicationUser(long id)
        {
            if((User.IsInRole("Administrator" ) || User.IsInRole("CustomerRepresentative")) == false)
            {
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == id.ToString() == false)
                {
                    return Unauthorized();
                }
            }
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByIdAsync(id.ToString()).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            applicationUser.Deleted = true;
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
                return Problem();
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
            ApplicationUser? applicationUser = _signInManager.UserManager
                .FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return false;
            }

            var changePasswordResult = await _signInManager.UserManager
                .ChangePasswordAsync(applicationUser, currentPassword, NewPassword);
            return changePasswordResult.Succeeded;
        }

        [HttpPost("PasswordReset")]
        public string? PasswordReset(string userName)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return null;
            }
            return _signInManager.UserManager.GeneratePasswordResetTokenAsync(applicationUser).Result;
        }

        [HttpPost("ValidateResetPassword")]
        public ActionResult<string> ValidateResetPassword(string UserName, string token, string newPassword)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager
                .FindByNameAsync(UserName).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            IdentityResult identityResult = _signInManager.UserManager
                .ResetPasswordAsync(applicationUser, token, newPassword).Result;
            if (!identityResult.Succeeded)
            {
                return identityResult.Errors.First().Description;
            }
            return Ok("Password Reset Successfull");
        }

        private struct movie
        {
            public Media media { get; set; }
            public int ViewCount { get; set; }
        }

        [HttpPost("LogIn")]
        public ActionResult<List<Media>> LogIn(LogInModel logInModel)
        {
            bool sonuc = false;
            IQueryable<User_Favorite> user_Favorites;
            IGrouping<int, Media_Category>? media_Categories;
            IQueryable<Media> mediasQuery;
            IQueryable<int> user_Watched;
            ApplicationUser? user = _signInManager.UserManager
                .FindByNameAsync(logInModel.userName).Result;
            if (user == null || user.Deleted == true)
            {
                return BadRequest(); 
            }
            try
            {
                Microsoft.AspNetCore.Identity.SignInResult signInResult =
                    _signInManager.PasswordSignInAsync(user, logInModel.password, false, false).Result;
                sonuc = signInResult.Succeeded;
            }
            catch (Exception)
            {
                return Problem();
            }
            if (sonuc != true)
            {
                return Problem("Invalid UserName or Password");
            }
            user_Favorites = _context.User_Favorites.Where(u => u.UserId == user.Id)
                .Include(u => u.Media).ThenInclude(u => u!.Media_Categories);

            media_Categories = user_Favorites.ToList().SelectMany(u => u.Media!.Media_Categories!)
                .GroupBy(m => m.CategoryId).OrderByDescending(m => m.Count()).FirstOrDefault();            
            if(media_Categories != null)
            {
                int fav_category = media_Categories.Key;
                user_Watched = _context.User_Watcheds.Where(u => u.UserId == user.Id).Include(u => u.Episode)
                    .Select(u=>u.Episode!.MediaId).Distinct();
                mediasQuery = _context.Medias.Include(m => m.Media_Categories!
                    .Where(mc => mc.CategoryId == fav_category))
                    .Where(w => user_Watched.Contains(w.Id) == false);
                if(user.Restriction != null)
                {
                    mediasQuery = mediasQuery.Include(m => m.Media_Restrictions.Where(r => r.RestrictionId <= user.Restriction));
                }
                List<movie> movies = new List<movie>();
                foreach(Media media in mediasQuery)
                {
                    movie movie = new movie();
                    int count = _context.User_Watcheds.Include(m=>m.Episode.Media).Where(m=>m.Episode.MediaId == media.Id).Count();
                    movie.ViewCount = count;
                    movie.media = media;
                    movies.Add(movie);
                }
                movies = movies.OrderByDescending(m => m.ViewCount).AsQueryable().ToList();
                List<Media> medias = new List<Media>();
                for (int i = 0; i < 5; i++)
                {
                    medias.Add(movies[i].media);
                }

                return medias;
            }

            return Problem();
            
        }

    }
}
