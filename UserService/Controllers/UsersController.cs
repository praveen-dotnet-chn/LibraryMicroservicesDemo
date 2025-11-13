using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly BookApiClient _bookClient;

        public UsersController(UserDbContext context, BookApiClient bookClient)
        {
            _context = context;
            _bookClient = bookClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<object>> GetAllUsers()
        {
            //var users = _context
            //    .Users.Include(u => u.BorrowedBooks)
            //    .Select(u => new
            //    {
            //        u.Id,
            //        u.Name,
            //        BorrowedBookIds = u.BorrowedBooks.Select(bb => bb.BookId).ToList(),
            //    })
            //    .ToList();

            //return Ok(users);
            return Ok(Request.Host);

        }

        [HttpPost]
        public ActionResult<User> AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, user);
        }

        [HttpPost("{id}/borrow/{bookId}")]
        public ActionResult BorrowBook(int id, int bookId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound("No User Found!!! Please Enter Correct User ID");

            var borrowed = new BorrowedBook { UserId = id, BookId = bookId };

            _context.BorrowedBooks.Add(borrowed);
            _context.SaveChanges();

            return Ok($"{user.Name} borrowed book {bookId}");
        }

        [HttpGet("{id}/borrowed")]
        public async Task<ActionResult<object>> GetBorrowedBooks(int id)
        {
            var user = _context.Users.Include(u => u.BorrowedBooks).FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound("No User Found!!! Please Enter Correct User ID");

            var books = new List<BookDto>();
            foreach (var borrowed in user.BorrowedBooks)
            {
                var book = await _bookClient.GetBookByIdAsync(borrowed.BookId);
                if (book != null)
                    books.Add(book);
            }

            return Ok(new { user.Name, BorrowedBooks = books });
        }
    }
}
