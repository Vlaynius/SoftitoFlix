using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Models;
using System.Security.Claims;
using SoftitoFlix.Dto.Requests.User;
using SoftitoFlix.Dto.Responses.User;

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

        public struct Movie
        {
            public Media media { get; set; }
            public int ViewCount { get; set; }
        }


        // GET: api/User
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public  ActionResult<List<GetUserResponse>> GetUsers(bool IncludePassiveUsers = true, bool IncludeDeletedUsers = true)
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
            List<ApplicationUser> list = users.AsNoTracking().ToList();
            List<GetUserResponse> response = new List<GetUserResponse>();
            foreach (ApplicationUser item in list)
            {
                GetUserResponse getUserResponse = new GetUserResponse();
                getUserResponse.BirthDate = item.BirthDate;
                getUserResponse.Phone = item.PhoneNumber;
                getUserResponse.Email = item.Email;
                getUserResponse.Name = item.Name;
                response.Add(getUserResponse);
            }
            return response;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        [Authorize]
        public  ActionResult<GetUserResponse> GetApplicationUser(GetUserID_Request request)
        {
            if(User.IsInRole("Administrator") == false)
            {
                if(User.FindFirstValue(ClaimTypes.NameIdentifier) == request.UserID.ToString() == false)
                {
                    return Unauthorized();
                }
            }
            if (_signInManager.UserManager.Users == null)
            {
                return NotFound();
            }
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByIdAsync(request.UserID.ToString()).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            GetUserResponse response = new GetUserResponse();
            response.BirthDate = applicationUser.BirthDate;
            response.Phone = applicationUser.PhoneNumber;
            response.Email = applicationUser.Email;
            response.Name = applicationUser.Name;
            return response;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult PutApplicationUser(long id, GetUserRequest request)
        {
            if (User.IsInRole("CustomerRepresentative") == false)
            {
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == id.ToString() == false)
                {
                    return Unauthorized();
                }
            }
            ApplicationUser? user = _signInManager.UserManager.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            user.Name = request.name;
            user.BirthDate = request.BirthDate;
            user.Email = request.email;
            user.PhoneNumber = request.phoneNumber;
            user.UserName = request.userName;
            _signInManager.UserManager.UpdateAsync(user).Wait();
            return Ok();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<object> PostApplicationUser(GetUserRequest request)
        {
            if(User.Identity!.IsAuthenticated == true)
            {
                return BadRequest();
            }
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.UserName = request.userName;
            applicationUser.PhoneNumber = request.phoneNumber;
            applicationUser.Email = request.email;
            applicationUser.BirthDate = request.BirthDate;
            applicationUser.Passive = false;
            applicationUser.Deleted = false;

            IdentityResult IdentityResult =_signInManager.UserManager.CreateAsync(applicationUser, request.password).Result;
            if(IdentityResult != IdentityResult.Success)
            {
                return IdentityResult.Errors.FirstOrDefault()!.Description;
            }
            return applicationUser.Id;
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteApplicationUser(GetUserID_Request request)
        {
            if((User.IsInRole("Administrator" ) || User.IsInRole("CustomerRepresentative")) == false)
            {
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == request.UserID.ToString() == false)
                {
                    return Unauthorized();
                }
            }
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByIdAsync(request.UserID.ToString()).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            applicationUser.Deleted = true;
            _signInManager.UserManager.UpdateAsync(applicationUser).Wait();
            return NoContent();
        }


        [Authorize(Roles = "Administrator")]
        [HttpPost("SetPassword")]
        public ActionResult<string> SetPassword(LogInModel logInModel)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(logInModel.userName).Result;
            if (applicationUser == null)
            {
                return Problem();
            }
            try
            {
                _signInManager.UserManager.RemovePasswordAsync(applicationUser).Wait();
                _signInManager.UserManager.AddPasswordAsync(applicationUser, logInModel.password).Wait();
            }
            catch (Exception)
            {
                return Ok("An Error Accured");
            }
            return Ok("Password Assigned Successfully");
        }

        
        [Authorize]
        [HttpPost("ChangePassword")]
        public bool ChangethePassword(ChangePassword change)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager
                .FindByNameAsync(change.UserName).Result;
            if (applicationUser == null)
            {
                return false;
            }

            IdentityResult changePasswordResult = _signInManager.UserManager
                .ChangePasswordAsync(applicationUser, change.CurrentPassword, change.NewPassword).Result;
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
        public ActionResult<string> ValidateResetPassword(LogInModel logInModel, string token)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager
                .FindByNameAsync(logInModel.userName).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            IdentityResult identityResult = _signInManager.UserManager
                .ResetPasswordAsync(applicationUser, token, logInModel.password).Result;
            if (!identityResult.Succeeded)
            {
                return identityResult.Errors.First().Description;
            }
            return Ok("Password Reset Successfull");
        }


        [HttpPost("LogIn")]
        public ActionResult<List<Media>> LogIn(LogInModel logInModel)
        {
            int i;
            IQueryable<User_Favorite> user_Favorites;
            IGrouping<int, Media_Category>? media_Categories;
            IQueryable<Media> mediasQuery;
            IQueryable<int> user_Watched;
            List<Media> medias = new List<Media>();
            List<Movie> movies = new List<Movie>();
            ApplicationUser? user = _signInManager.UserManager
                .FindByNameAsync(logInModel.userName).Result;
            Microsoft.AspNetCore.Identity.SignInResult signInResult;


            if (user == null || user.Deleted == true)
            {
                return BadRequest(); 
            }

            if (_signInManager.UserManager.IsInRoleAsync(user, "Administrator").Result == true ||
                _signInManager.UserManager.IsInRoleAsync(user, "ContentAdmin").Result == true ||
                _signInManager.UserManager.IsInRoleAsync(user, "CustomerRepresentative").Result == true)
            {
               signInResult = _signInManager.PasswordSignInAsync(user, logInModel.password, false, false).Result;
                if (signInResult.Succeeded)
                {
                    return Ok("Admin LoggedIn");
                }
            }

            if (_context.User_Plans.Where(u => u.UserId == user.Id && u.EndDate >= DateTime.Today).Any() == false)
            {
                user.Passive = true;
                _signInManager.UserManager.UpdateAsync(user).Wait();
            }
            if (user.Passive == true)
            {
                return Content("Passive");
            }
            signInResult = _signInManager.PasswordSignInAsync(user, logInModel.password, false, false).Result;
           
            if (signInResult.Succeeded == false)
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
                mediasQuery = _context.Medias.Include(m => m.Media_Categories!.Where(mc => mc.CategoryId == fav_category))
                    .Where(m => m.Media_Categories!.Count() > 0).Where(w => user_Watched.Contains(w.Id) == false);
                if (user.Restriction != null)
                {
                    mediasQuery = mediasQuery.Include(m => m.Media_Restrictions!.Where(r => r.RestrictionId <= user.Restriction));
                }
            }
            else
            {   //Favoriler boşsa yeni bir kullanıcıysa
                mediasQuery = _context.Medias.Include(m => m.Media_Restrictions!.Where(r => r.RestrictionId <= user.Restriction));
            }
            
            foreach (Media media in mediasQuery)
            {
                Movie movie = new Movie();
                int count = _context.User_Watcheds.Include(m => m.Episode!.Media).Where(m => m.Episode!.MediaId == media.Id).Count();
                movie.ViewCount = count;
                movie.media = media;
                movies.Add(movie);
            }
            movies = movies.OrderByDescending(m => m.ViewCount).AsQueryable().ToList();
            for (i = 0; i < 5 && i< movies.Count(); i++)
            {
                medias.Add(movies[i].media);
            }

            return medias;
        }

        [HttpPost("Logout")]
        [Authorize]
        public void LogOut()
        {
            _signInManager.SignOutAsync().Wait();
        }


    }
}
