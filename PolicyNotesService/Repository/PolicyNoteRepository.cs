using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Data;
using PolicyNotesService.Models;

namespace PolicyNotesService.Repository
{
    public class PolicyNoteRepository : IPolicyNoteRepository
    {
        private readonly NotesDbContext _db;
        public PolicyNoteRepository(NotesDbContext db)
        {
            this._db = db;
        }
        public async Task<PolicyNote> AddAsync(PolicyNote note)
        {
            _db.PolicyNotes.Add(note);
            await _db.SaveChangesAsync();
            return note;
        }

        public Task<List<PolicyNote>> GetAllAsync() => _db.PolicyNotes.ToListAsync();

        public Task<PolicyNote?> GetByIdAsync(int id) => _db.PolicyNotes.FirstOrDefaultAsync(n => n.Id == id);
    }
}
