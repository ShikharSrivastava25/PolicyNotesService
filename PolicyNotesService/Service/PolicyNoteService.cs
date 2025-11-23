using PolicyNotesService.Dto;
using PolicyNotesService.Models;
using PolicyNotesService.Repository;

namespace PolicyNotesService.Service
{
    public class PolicyNoteService : IPolicyNotesService
    {
        private readonly IPolicyNoteRepository _repo;

        public PolicyNoteService(IPolicyNoteRepository repo)
        {
            _repo = repo;
        }
        public async Task<PolicyNoteDto> AddNoteAsync(PolicyNoteCreateDto dto)
        {
            var entity = new PolicyNote
            {
                PolicyNumber = dto.PolicyNumber,
                Note = dto.Note
            };

            var created = await _repo.AddAsync(entity);

            return new PolicyNoteDto
            {
                Id = created.Id,
                PolicyNumber = created.PolicyNumber,
                Note = created.Note
            };
        }

        public async Task<List<PolicyNoteDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();

            return list.Select(n => new PolicyNoteDto
            {
                Id = n.Id,
                PolicyNumber = n.PolicyNumber,
                Note = n.Note
            }).ToList();
        }

        public async Task<PolicyNoteDto?> GetByIdAsync(int id)
        {
            var found = await _repo.GetByIdAsync(id);
            return found == null ? null : new PolicyNoteDto { Id = found.Id, PolicyNumber = found.PolicyNumber, Note = found.Note };
        }
    }
}
