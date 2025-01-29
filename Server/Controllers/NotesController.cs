using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly Database _database;

        public NotesController(Database database)
        {
            _database = database;
        }

        // GET: Notes/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            if (HttpContext.Session.GetString("username") == null)
                return Unauthorized("Not logged in");

            var username = HttpContext.Session.GetString("username");
            try
            {
                return await _database.Note.Where(note => note.Username == username).ToListAsync();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        // GET: Notes/:id
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(long id)
        {
            if (HttpContext.Session.GetString("username") == null)
                return Unauthorized("Not logged in");
            try
            {
                var note = await _database.Note.FindAsync(id);

                if (note == null || note.Username != HttpContext.Session.GetString("username"))
                {
                    return NotFound("Note not found");
                }

                return note;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        // PUT: Notes/:id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote(Note note)
        {
            if (HttpContext.Session.GetString("username") == null)
                return Unauthorized("Not logged in");

            try
            {
                var noteDB = await _database.Note.FindAsync(note.IdNote);

                if (noteDB == null || noteDB.Username != HttpContext.Session.GetString("username"))
                {
                    return NotFound("Note not found");
                }

                noteDB.Content = note.Content;
                noteDB.Date = note.Date;

                await _database.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        // POST: Notes/
        [HttpPost]
        public async Task<ActionResult<Note>> PostNote(Note note)
        {
            if (HttpContext.Session.GetString("username") == null)
                return Unauthorized("Not logged in");

            if (note.Username != HttpContext.Session.GetString("username"))
            {
                return Unauthorized("Invalid user");
            }

            try
            {
                _database.Note.Add(note);
                await _database.SaveChangesAsync();

                return Ok(note);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return StatusCode(500);
            }
        }
        
        // DELETE: Notes/:id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(long id)
        {
            if (HttpContext.Session.GetString("username") == null)
                return Unauthorized("Not logged in");

            try
            {
                var note = await _database.Note.FindAsync(id);

                if (note == null || note.Username != HttpContext.Session.GetString("username"))
                {
                    return NotFound("Note not found");
                }

                _database.Note.Remove(note);
                await _database.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return StatusCode(500);
            }
        }
    }
}
