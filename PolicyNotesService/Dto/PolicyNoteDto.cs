namespace PolicyNotesService.Dto
{
    public record PolicyNoteDto
    {
        public int Id { get; init; }
        public string PolicyNumber { get; init; } = string.Empty;
        public string Note { get; init; } = string.Empty;
    }
}
