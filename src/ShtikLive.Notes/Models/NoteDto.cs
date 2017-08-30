using System;
using ShtikLive.Notes.Data;

namespace ShtikLive.Notes.Models
{
    public class NoteDto
    {
        public string Text { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public static NoteDto FromNote(Note note)
        {
            return new NoteDto
            {
                Text = note.NoteText,
                Timestamp = note.Timestamp
            };
        }
    }
}