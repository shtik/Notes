using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ShtikLive.Notes.Data
{
    public class NoteContext : DbContext
    {
        public NoteContext([NotNull] DbContextOptions options) : base(options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }

        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Note>()
                .HasIndex(n => new {n.UserHandle, n.SlideIdentifier})
                .IsUnique();
        }
    }
}
