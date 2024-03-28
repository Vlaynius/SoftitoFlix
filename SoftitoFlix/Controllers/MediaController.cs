using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Models;

namespace SoftitoFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MediaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Media
        [HttpGet]
        [Authorize("ContentAdmin")]
        public ActionResult<List<Media>> GetMedias()
        {
            return _context.Medias.ToList();
        }

        // GET: api/Media/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Media> GetMedia(int id)
        {
            Media? media = _context.Medias.Where(m=>m.Id == id).FirstOrDefault(m => m.Passive == true);
            if (media == null)
            {
                return NotFound();
            }
            return media;
        }

        // PUT: api/Media/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize("ContentAdmin")]
        public void PutMedia( Media media)
        {
            _context.Entry(media).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            { }
        }

        // POST: api/Media
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize("ContentAdmin")]
        public int PostMedia(Media media)
        {
            _context.Medias.Add(media);
            _context.SaveChanges();
            return media.Id;
        }

        // DELETE: api/Media/5
        [HttpDelete("{id}")]
        [Authorize("ContentAdmin")]
        public ActionResult DeleteMedia(int id)
        {
            Media? media = _context.Medias.Find(id);
            if (media == null)
            {
                return NotFound();
            }
            media.Passive = false;
            _context.SaveChanges();

            return NoContent();
        }

    }
}
