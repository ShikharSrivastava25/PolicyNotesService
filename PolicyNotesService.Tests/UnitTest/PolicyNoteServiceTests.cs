using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Data;
using PolicyNotesService.Dto;
using PolicyNotesService.Repository;
using PolicyNotesService.Service;

namespace PolicyNotesService.Tests.UnitTest
{
    public class PolicyNoteServiceTests
    {
        private PolicyNoteService CreateService(string dbName)
        {
            var options = new DbContextOptionsBuilder<NotesDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var db = new NotesDbContext(options);
            var repo = new PolicyNoteRepository(db);
            return new PolicyNoteService(repo);
        }

        [Fact]
        public async Task AddNote_AddsNoteAndReturnsDto()
        {
            var service = CreateService("AddNoteTest");
            var dto = new PolicyNoteCreateDto
            {
                PolicyNumber = "P-111",
                Note = "Testing add"
            };

            var result = await service.AddNoteAsync(dto);

            Assert.True(result.Id > 0);
            Assert.Equal(dto.PolicyNumber, result.PolicyNumber);
            Assert.Equal(dto.Note, result.Note);
        }

        [Fact]
        public async Task GetAll_ReturnsAllNotes()
        {
            var service = CreateService("GetAllTest");

            await service.AddNoteAsync(new PolicyNoteCreateDto { PolicyNumber = "P1", Note = "Note 1" });
            await service.AddNoteAsync(new PolicyNoteCreateDto { PolicyNumber = "P2", Note = "Note 2" });

            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count);
        }
    }
}
