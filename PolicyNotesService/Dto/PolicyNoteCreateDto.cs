namespace PolicyNotesService.Dto
{
    public record PolicyNoteCreateDto
    {
        public string PolicyNumber { get; init; } = string.Empty;
        public string Note { get; init; } = string.Empty;
    }
}
