namespace NoteFlow.Domain.Entities
{
    public class Note : EntityBase
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsArchived { get; set; }
        public bool IsPinned { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
