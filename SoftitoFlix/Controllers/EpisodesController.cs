using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SoftitoFlix.Data;
using SoftitoFlix.Models;

namespace SoftitoFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public EpisodesController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: api/Episodes
        [HttpGet]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult<List<Episode>> GetEpisodes(int mediaId, byte seasonNumber)
        {

            return _context.Episodes.Where(e => e.MediaId == mediaId && e.SeasonNumber == seasonNumber).OrderBy(e => e.EpisodeNumber).ToList();
        }

        // GET: api/Episodes/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Episode> GetEpisode(long id)
        {
            Episode? episode = _context.Episodes.Find(id);
            if (episode == null)
            {
                return NotFound();
            }
            return episode;
        }

        [HttpGet("Watch")]
        [Authorize]
        public ActionResult Watch(long id)
        {
            ApplicationUser user = _signInManager.UserManager.GetUserAsync(User).Result!;
            User_Watched userWatched = new User_Watched();
            Episode? episode = _context.Episodes.Find(id);

            if(episode == null)
            {
                return NotFound();
            }

            try
            {
                userWatched.UserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);//Giriş yapan kullanıcının idsini longa çevirim UserId'ye eşitledik
                userWatched.EpisodeId = id;
                _context.User_Watcheds.Add(userWatched);
                episode.ViewCount++;
                _context.Episodes.Update(episode);
                _context.SaveChanges();
                
            }catch (Exception ){ }

            return Accepted();
        }

        // PUT: api/Episodes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public void PutEpisode(Episode episode)
        {
            _context.Entry(episode).State = EntityState.Modified;
            try
            {
                 _context.SaveChanges();
            }
            catch (Exception)
            { }
        }

        // POST: api/Episodes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "ContentAdmin")]
        public long PostEpisode(Episode episode)
        {
            _context.Episodes.Add(episode);
             _context.SaveChanges();
            return episode.Id;
        }

        // DELETE: api/Episodes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeleteEpisode(long id)
        {
            Episode? episode = _context.Episodes.Find(id);
            if (episode == null)
            {
                return NotFound();
            }
            episode.Passive = true;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("Activate")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult ActivateEpisode(long id)
        {
            Episode? episode = _context.Episodes.Find(id);
            if (episode == null)
            {
                return NotFound();
            }
            episode.Passive = false;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost("Favorite")]
        [Authorize]
        public void AddtoFavorite(int mediaId)
        {
            try
            {
                User_Favorite user_Favorite = new User_Favorite();
                user_Favorite.MediaId = mediaId;
                user_Favorite.UserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                _context.User_Favorites.Add(user_Favorite);
                _context.SaveChanges();
            }
            catch (Exception) { }

        }

        [HttpDelete("Favorite")]
        [Authorize]
        public ActionResult RemovefromFavorite(int mediaId)
        {
            long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            User_Favorite? user_Favorite = _context.User_Favorites.Where(m => m.UserId == userId).FirstOrDefault(m => m.MediaId == mediaId);
            if(user_Favorite == null)
            {
                return NotFound();
            }
            _context.User_Favorites.Remove(user_Favorite);
            _context.SaveChanges();
            return Accepted();
        }

    }
}
