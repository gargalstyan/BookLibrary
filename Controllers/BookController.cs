using BookLibrary.Data;
using BookLibrary.Data.Entities;
using BookLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Controllers
{
    //[Authorize]
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
                        Id = book.ID,
                        Name = book.Name,
                        Genre = book.Genre,
                        Count = book.Count,
                        Year = book.Year,
                        Publisher = book.Publisher.Name,
                        AuthorsName = book.Authors.Select(a => a.Name).ToList(),
                        Price = book.Price,
                    });
                }
            }

            return View(bookModels);
        }

        public IActionResult Edit(int id)
        {
            if (id == 0)
                NotFound();
            var editbook = _applicationDbContext.Books
                .Include(a => a.Authors)
                .Include(p => p.Publisher).FirstOrDefault(i => i.ID == id);

            BookModel model = new BookModel
            {
                Id = editbook.ID,
                Name = editbook.Name,
                Genre = editbook.Genre,
                Count = editbook.Count,
                Year = editbook.Year,
                Location = editbook.Publisher.Location,
                Publisher = editbook.Publisher.Name,
                /////////////////////////////////////
                AuthorsName = editbook.Authors.Select(a => a.Name).ToList(),
                Price = editbook.Price,
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(BookModel bookModel)
        {
            var editbook = _applicationDbContext.Books
               .Include(a => a.Authors)
               .Include(p => p.Publisher).FirstOrDefault(i => i.ID == bookModel.Id);
            editbook.Name = bookModel.Name;
            editbook.Genre = bookModel.Genre;
            editbook.Count = bookModel.Count;
            editbook.Publisher.Location = bookModel.Location;
            editbook.Publisher.Name = bookModel.Publisher;
            editbook.Price = bookModel.Price;

            for (int i = 0; i < editbook.Authors.Count; i++)
            {
                //editbook.Authors[i].Name = bookModel.Authors[i].Name;
                editbook.Authors[i].Name = bookModel.AuthorsName[i];
            }

            _applicationDbContext.Update(editbook);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
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
                foreach (var item in bookModel.AuthorsName)
                {
                    book.Authors.Add(new Author { Name = item, BirthData = 1988 });
                }

                var publisher = _applicationDbContext.Publishers.FirstOrDefault(p => p.Name == bookModel.Publisher && p.Location == bookModel.Location);
                if (publisher != null)
                {
                    book.Publisher = publisher;
                }
                else
                {
                    book.Publisher = new Publisher { Name = bookModel.Publisher, Location = bookModel.Location };
                }

                _applicationDbContext.Books.Add(book);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            {
                return View();
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var editbook = _applicationDbContext.Books
               .Include(a => a.Authors)
               .Include(p => p.Publisher).FirstOrDefault(i => i.ID == id);
            if (editbook == null)
                return NotFound();
            _applicationDbContext.Books.Remove(editbook);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Info(int id)
        {
            var editbook = _applicationDbContext.Books
                .Include(a => a.Authors)
                .Include(p => p.Publisher).FirstOrDefault(i => i.ID == id);
            if (editbook != null)
                return View(editbook.Authors);
            else
                return NotFound();

        }
    }
}
