namespace NoteFlow.Domain.Entities
{
    public class User : EntityBase
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsEmailVerified { get; set; }

        public List<Note> Notes { get; set; }
    }
}
