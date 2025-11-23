using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Data;
using PolicyNotesService.Dto;
using PolicyNotesService.Repository;
using PolicyNotesService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

if (!builder.Environment.IsEnvironment("Testing"))  
{
    builder.Services.AddDbContext<NotesDbContext>(options => options.UseInMemoryDatabase("PolicyNotesDb"));
}

builder.Services.AddScoped<IPolicyNoteRepository, PolicyNoteRepository>();
builder.Services.AddScoped<IPolicyNotesService, PolicyNoteService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/notes", async (PolicyNoteCreateDto dto, IPolicyNotesService service) =>
{
    if (string.IsNullOrWhiteSpace(dto.PolicyNumber) ||
        string.IsNullOrWhiteSpace(dto.Note))
    {
        return Results.BadRequest("PolicyNumber and note are required.");
    }

    var created = await service.AddNoteAsync(dto);
    return Results.Created($"/notes/{created.Id}", created);
});

app.MapGet("/notes", async (IPolicyNotesService service) =>
{
    var all = await service.GetAllAsync();
    return Results.Ok(all);
});

app.MapGet("/notes/{id:int}", async (int id, IPolicyNotesService service) =>
{
    var note = await service.GetByIdAsync(id);
    return note is null ? Results.NotFound() : Results.Ok(note);
});

app.UseHttpsRedirection();

if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
        db.Database.EnsureCreated();
    }
}

app.Run();

public partial class Program { }