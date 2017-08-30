using System;
using System.ComponentModel.DataAnnotations;

namespace ShtikLive.Notes.Data
{
    public class Note
    {
        public int Id { get; set; }

        [MaxLength(256)]
        public string SlideIdentifier { get; set; }

        [MaxLength(16)]
        public string UserHandle { get; set; }

        public string NoteText { get; set; }

        public bool Public { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}