using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Plugins;
using SoftitoFlix.Data;
using SoftitoFlix.Dto.Request.Episodes;
using SoftitoFlix.Dto.Request.Media;
using SoftitoFlix.Dto.Response.Episodes;
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
        public ActionResult<GetEpisodesResponse> GetEpisodes(GetEpisodesRequest request)
        {
            GetEpisodesResponse response = new GetEpisodesResponse();
            List<Episode> episodes = _context.Episodes.Where(e => e.MediaId == request.MediaId && e.SeasonNumber == request.SeasonId).OrderBy(e => e.EpisodeNumber).ToList();
            if(episodes == null)
            {
                return NotFound();
            }
            foreach(Episode episode in episodes)
            {
                GetEpisodeResponse response1 = new GetEpisodeResponse();
                response1.Description = episode.Description;
                response1.Duration = episode.Duration;
                response1.EpisodeNumber = episode.EpisodeNumber;
                response1.ReleaseDate = episode.ReleaseDate;
                response1.SeasonNumber = episode.SeasonNumber;
                response1.Title = episode.Title;
                response1.Media!.Description = episode.Media!.Description;
                response1.Media.Name = episode.Media.Name;
                response1.Media.Rating = episode.Media.Rating;
                response1.Media.ReleaseDate = episode.Media.ReleaseDate;
                response.Episodes!.Add(response1);
            }
            return response;
        }

        // GET: api/Episodes/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<GetEpisodeResponse> GetEpisode(EpisodeID_Request request)
        {
            Episode? episode = _context.Episodes.Find(request.Id);
            if (episode == null)
            {
                return NotFound();
            }
            GetEpisodeResponse response = new GetEpisodeResponse();
            response.Description = episode.Description;
            response.Duration = episode.Duration;
            response.EpisodeNumber = episode.EpisodeNumber;
            response.ReleaseDate = episode.ReleaseDate;
            response.SeasonNumber = episode.SeasonNumber;
            response.Title = episode.Title;
            response.Media!.Description = episode.Media!.Description;
            response.Media.Name = episode.Media.Name;
            response.Media.Rating = episode.Media.Rating;
            response.Media.ReleaseDate = episode.Media.ReleaseDate;
            return response;
        }

        [HttpGet("Watch")]
        [Authorize]
        public ActionResult Watch(EpisodeID_Request request)
        {
            User_Watched userWatched = new User_Watched();
            Episode? episode = _context.Episodes.Find(request.Id);

            if(episode == null)
            {
                return NotFound();
            }

            try
            {
                userWatched.UserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                userWatched.EpisodeId = request.Id;
                _context.User_Watcheds.Add(userWatched);
                episode.ViewCount++;
                _context.Episodes.Update(episode);
                _context.SaveChanges();
                
            }catch (Exception ){ }

            return Accepted();
        }

        // PUT: api/Episodes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult PutEpisode(PutEpisodeRequest request)
        {
            Episode? episode = _context.Episodes.Where(e => e.MediaId == request.MediaId).Where(e => e.SeasonNumber == request.SeasonNumber)
                .Where(e => e.EpisodeNumber == request.EpisodeNumber).FirstOrDefault();
            if(episode == null)
            {
                return NotFound();
            }
            _context.Entry(episode).State = EntityState.Modified;
            try
            {
                 _context.SaveChanges();
            }
            catch (Exception)
            { }
            return Ok();
        }

        // POST: api/Episodes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "ContentAdmin")]
        public long PostEpisode(PostEpisodeRequest request)
        {
            Episode episode = new Episode();
            episode.Description = request.Description;
            episode.Duration = request.Duration;
            episode.EpisodeNumber = request.EpisodeNumber;
            episode.MediaId = request.MediaId;
            episode.Passive = false;
            episode.ReleaseDate = request.ReleaseDate;
            episode.SeasonNumber = request.SeasonNumber;
            episode.Title = request.Title;
            episode.ViewCount = 0;
            _context.Episodes.Add(episode);
             _context.SaveChanges();
            return episode.Id;
        }

        // DELETE: api/Episodes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeleteEpisode(EpisodeID_Request request)
        {
            Episode? episode = _context.Episodes.Find(request.Id);
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
        public ActionResult ActivateEpisode(EpisodeID_Request request)
        {
            Episode? episode = _context.Episodes.Find(request.Id);
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
        public void AddtoFavorite(MediaID_Request request)
        {
            try
            {
                User_Favorite user_Favorite = new User_Favorite();
                user_Favorite.MediaId = request.Id;
                user_Favorite.UserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                _context.User_Favorites.Add(user_Favorite);
                _context.SaveChanges();
            }
            catch (Exception) { }

        }

        [HttpDelete("Favorite")]
        [Authorize]
        public ActionResult RemovefromFavorite(MediaID_Request request)
        {
            long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            User_Favorite? user_Favorite = _context.User_Favorites.Where(m => m.UserId == userId).FirstOrDefault(m => m.MediaId == request.Id);
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
