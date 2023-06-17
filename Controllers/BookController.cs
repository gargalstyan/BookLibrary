using BookLibrary.Data;
using BookLibrary.Data.Entities;
using BookLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public BookController(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            List<BookModel> bookModels = new List<BookModel>();
            var books = _applicationDbContext.Books
                .Include(a => a.Authors)
                .Include(p => p.Publisher);

            if (books != null)
            {


                foreach (var book in books)
                {
                    bookModels.Add(new BookModel
                    {
                        Name = book.Name,
                        Genre = book.Genre,
                        Count = book.Count,
                        Year = book.Year,
                        Publisher = book.Publisher.Name,
                        Authors = book.Authors.Select(a => a.Name).ToList(),
                        Price = book.Price,
                    });
                }
            }

            return View(bookModels);
        }
        public IActionResult Add()
        {
            BookModel bookModel = new BookModel();
            return View(bookModel);
        }
        [HttpPost]
        public async Task<IActionResult> Add(BookModel bookModel)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Name = bookModel.Name,
                    Genre = bookModel.Genre,
                    Year = bookModel.Year,
                    Count = bookModel.Count,
                    Price = bookModel.Price,
                };
                foreach (var item in bookModel.Authors)
                {
                    book.Authors.Add(new Author { Name = item });
                }
                var publisher = new Publisher
                {
                    Location = bookModel.Location,
                    Name = bookModel.Publisher,

                };
                book.Publisher = publisher;
                _applicationDbContext.Books.Add(book);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            {
                return View();
            }
        }
    }
}
