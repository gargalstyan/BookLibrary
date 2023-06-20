using BookLibrary.Data;
using BookLibrary.Data.Entities;
using BookLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
           
            var books = _applicationDbContext.Books
               
                .Select(book => new BookModel
                {
                    Id = book.ID,
                    Name = book.Name,
                    Genre = book.Genre,
                    Count = book.Count,
                    Year = book.Year,
                    Publisher = book.Publisher.Name,
                    Authors = book.Authors.Select(a => a.Name).ToList(),
                    Price = book.Price,

                }).ToList(); 


            return View(books);
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


        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    BookModel? record = await _applicationDbContext.Books.FindAsync(id); // Read/Write (IO)

        //    await _applicationDbContext.SaveChangesAsync();

        //    return View(record);
        //}


        [HttpGet]
        public IActionResult Edit(int id)
        {
            BookModel model;
            var book = _applicationDbContext.Books.FirstOrDefault(b=> b.ID== id);
            model = new BookModel()
            {
                Id = book.ID,
                Name = book.Name,
                Genre = book.Genre,
                Count = book.Count,
                Year = book.Year,
                Publisher = book.Publisher.Name,
                Authors = book.Authors.Select(a => a.Name).ToList(),
                Price = book.Price
                
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(BookModel model)
        {
            var post = _applicationDbContext.Books.FirstOrDefault(b => b.ID == model.Id);
            if(post != null)
            post.Name = model.Name;

            _applicationDbContext.Entry(post).State = EntityState.Modified;
           
           
            _applicationDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public async Task<IActionResult> Edit(EmployeeViewModel record)
        //{

        //    _applicationDbContext.Books.Update(record);

        //    await _applicationDbContext.SaveChangesAsync();

        //    return RedirectToAction("Index");
        //}


        //public async Task<IActionResult> Delete(int id)
        //{
        //    var record = await _applicationDbContext.Books.FindAsync(id);

        //    if (record == null)
        //        return NotFound();

        //    _applicationDbContext.Books.Remove(record);

        //    await _applicationDbContext.SaveChangesAsync();

        //    return RedirectToAction("Index");
        //}
        [HttpGet]
        public IActionResult Delete(int id)
        {

            return View(new BookModel { Id = id });
        }

        [HttpPost]
        public IActionResult Delete(BookModel model)
        {
            var item = _applicationDbContext.Books.FirstOrDefault(b => b.ID == model.Id);

            if (item == null)
            
                return NotFound();


                _applicationDbContext.Books.Remove(item);

                _applicationDbContext.SaveChanges();

                return RedirectToAction(nameof(Index));           
          
        }

    }

   


}
