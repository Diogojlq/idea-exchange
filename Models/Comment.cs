namespace IdeaExchange.Models
{
    public class Comment
    {
            public int Id { get; set; }
            public string Content { get; set; }
            public DateTime Date { get; set; }
            public int PublicationId { get; set; }
            public string UserId { get; set; }
    }
}
