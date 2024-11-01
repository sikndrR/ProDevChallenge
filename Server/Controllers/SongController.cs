using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using music_manager_starter.Data;
using music_manager_starter.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace music_manager_starter.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly DataDbContext _context;

        public SongsController(DataDbContext context)
        {
            _context = context;
        }

        // Get all songs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
        {
            return await _context.Songs.ToListAsync();
        }

        // Get a song by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Song>> GetSongById(Guid id)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            return Ok(song);
        }

        //Add Album cover image through ID
        [HttpPost("{id}/cover")]
        public async Task<IActionResult> UploadAlbumCover(Guid id, [FromBody] byte[] coverImage)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            // Save the BLOB data in the database
            song.Cover = coverImage;
            await _context.SaveChangesAsync();

            return Ok();
        }


        // Add a new song
        [HttpPost]
        public async Task<ActionResult<Song>> PostSong(Song song)
        {
            if (song == null)
            {
                return BadRequest("Song cannot be null.");
            }

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
