using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShtikLive.Notes.Data;
using ShtikLive.Notes.Models;

namespace ShtikLive.Notes.Controllers
{
    using static ResultMethods;

    public class NotesController
    {
        private readonly ILogger<NotesController> _logger;
        private readonly NoteContext _context;

        public NotesController(NoteContext context, ILogger<NotesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{user}/{presenter}/{slug}/{number}")]
        public async Task<IActionResult> GetForSlide(string user, string presenter, string slug, int number, CancellationToken ct)
        {
            var slideIdentifier = $"{presenter}/{slug}/{number}";
            try
            {
                var existingNote = await _context.Notes
            .SingleOrDefaultAsync(n => n.UserHandle == user && n.SlideIdentifier == slideIdentifier, ct)
            .ConfigureAwait(false);

            return existingNote == null ? NotFound() : Ok(NoteDto.FromNote(existingNote));
            }
            catch (Exception ex)
            {
                _logger.LogError(EventIds.DatabaseError, ex, ex.Message);
                throw;
            }
        }

        [HttpPut("{user}/{presenter}/{slug}/{number}")]
        public async Task<IActionResult> SetForSlide(string user, string presenter, string slug, int number, [FromBody] NoteDto note, CancellationToken ct)
        {
            var slideIdentifier = $"{presenter}/{slug}/{number}";
            var existingNote = await _context.Notes
                .SingleOrDefaultAsync(n => n.UserHandle == user && n.SlideIdentifier == slideIdentifier, ct)
                .ConfigureAwait(false);
            if (existingNote == null)
            {
                existingNote = new Note
                {
                    Public = false,
                    SlideIdentifier = slideIdentifier,
                    UserHandle = user
                };
                _context.Notes.Add(existingNote);
            }
            existingNote.NoteText = note.Text;
            existingNote.Timestamp = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return Accepted();
        }
    }
}
