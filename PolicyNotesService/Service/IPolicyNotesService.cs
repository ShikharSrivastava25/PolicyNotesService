using PolicyNotesService.Dto;

namespace PolicyNotesService.Service
{
    public interface IPolicyNotesService
    {
        Task<PolicyNoteDto> AddNoteAsync(PolicyNoteCreateDto dto);
        Task<List<PolicyNoteDto>> GetAllAsync();
        Task<PolicyNoteDto?> GetByIdAsync(int id);
    }
}
