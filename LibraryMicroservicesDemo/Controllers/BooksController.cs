using BookService.Data;
using BookService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookDbContext _context;

        public BooksController(BookDbContext context)
        {
            _context = context;
        }

        //Getting all books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            //return Ok(_context.Books.ToList());
            return Ok(Request.Host);
        }

        //Getting a book by id here

        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id)
        {
            var book = _context.Books.Find(id);
            return book == null ? NotFound("OOPS! Book not Found :( ") : Ok(book);
        }

        //Adding a new book using POST method
        [HttpPost]
        public ActionResult<Book> AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            //Here, Im Sending a response with the location of the newly created book
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }
    }
}
