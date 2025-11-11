namespace UserService.Models
{
    public class BorrowedBook
    {
        public int Id { get; set; }              // PK
        public int BookId { get; set; }          // Reference to the Book (from BookService)
        public int UserId { get; set; }          // FK to User

        // Navigation property
        public User? User { get; set; }
    }
}
