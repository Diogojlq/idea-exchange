namespace IdeaExchange.Models
{
    public class Publication
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? Date { get; set; } = default(DateTime);
        public string? AuthorId { get; set; }
        public ApplicationUser? Author { get; set; }

    }
}
