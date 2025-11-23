using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Models;

namespace PolicyNotesService.Data
{
    public class NotesDbContext(DbContextOptions<NotesDbContext> options) : DbContext(options)
    {
        public DbSet<PolicyNote> PolicyNotes => Set<PolicyNote>();
    }
}
