using PolicyNotesService.Models;

namespace PolicyNotesService.Repository
{
    public interface IPolicyNoteRepository
    {
        Task<PolicyNote> AddAsync(PolicyNote note);
        Task<List<PolicyNote>> GetAllAsync();
        Task<PolicyNote?> GetByIdAsync(int id);
    }
}
