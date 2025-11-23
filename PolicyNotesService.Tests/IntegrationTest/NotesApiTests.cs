using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using PolicyNotesService.Data;
using PolicyNotesService.Dto;
using System.Net;
using System.Net.Http.Json;

namespace PolicyNotesService.Tests.IntegrationTest
{
    public class NotesApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public NotesApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private HttpClient CreateClientWithInMemoryDb(bool clearDatabase)
        {
            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<NotesDbContext>));

                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<NotesDbContext>(options => options.UseInMemoryDatabase("TestDb"));
                });
            });

            if (clearDatabase)
            {
                using var scope = factory.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
                db.PolicyNotes.RemoveRange(db.PolicyNotes);
                db.SaveChanges();
            }

            return factory.CreateClient();
        }

        [Fact]
        public async Task PostNotes_Returns201Created()
        {
            var client = CreateClientWithInMemoryDb(true);
            var dto = new PolicyNoteCreateDto
            {
                PolicyNumber = "P-200",
                Note = "Sample note"
            };

            var response = await client.PostAsJsonAsync("/notes", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var created = await response.Content.ReadFromJsonAsync<PolicyNoteDto>();

            Assert.NotNull(created);
            Assert.True(created!.Id > 0);
            Assert.Equal(dto.PolicyNumber, created.PolicyNumber);
        }

        [Fact]
        public async Task GetNotes_Returns200Ok()
        {
            var client = CreateClientWithInMemoryDb(true);

            await client.PostAsJsonAsync("/notes",
                new PolicyNoteCreateDto { PolicyNumber = "PX", Note = "Seed note" });

            var response = await client.GetAsync("/notes");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var notes = await response.Content.ReadFromJsonAsync<List<PolicyNoteDto>>();
            Assert.NotNull(notes);
            Assert.True(notes!.Count > 0);
        }

        [Fact]
        public async Task GetNoteById_Returns200WhenFound_404WhenMissing()
        {
            var client = CreateClientWithInMemoryDb(true);

            var dto = new PolicyNoteCreateDto
            {
                PolicyNumber = "P-300",
                Note = "Test note"
            };

            var post = await client.PostAsJsonAsync("/notes", dto);
            var created = await post.Content.ReadFromJsonAsync<PolicyNoteDto>();

            Assert.NotNull(created);

            var getExisting = await client.GetAsync($"/notes/{created!.Id}");
            Assert.Equal(HttpStatusCode.OK, getExisting.StatusCode);

            var getMissing = await client.GetAsync("/notes/9999");
            Assert.Equal(HttpStatusCode.NotFound, getMissing.StatusCode);
        }
    }
}
