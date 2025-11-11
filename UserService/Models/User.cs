using System.Text.Json.Serialization;

namespace UserService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        // Navigation property — one user can have many borrowed books
        [JsonIgnore]
        public ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();
    }
}
