﻿using BookLibrary.Data;
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
                        Authors = book.Authors,
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
                Authors = editbook.Authors,
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
                editbook.Authors[i].Name = bookModel.Authors[i].Name;
                editbook.Authors[i].BirthData = bookModel.Authors[i].BirthData;
            }

            _applicationDbContext.Update(editbook);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Add()
        {
            BookModel bookModel = new BookModel();
            bookModel.Authors = _applicationDbContext.Authors.ToList();
            bookModel.Publishers = _applicationDbContext.Publishers.ToList();
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
                var authorsId = bookModel.Authors.Select(a => a.ID).ToList();   
                var author = _applicationDbContext.Authors.Where(a =>authorsId.Contains(a.ID)).ToList();
                book.Authors = author;

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
            editbook.Deleted = true;
            if (editbook == null)
                return NotFound();
            _applicationDbContext.Books.Update(editbook);
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
        public IActionResult Search(string searchParam)
        {
            if (searchParam == null)
                return NotFound();
            var books = _applicationDbContext.Books
                  .Include(a => a.Authors).Include(p=>p.Publisher);
            //var authors = books.Select(a => a.Authors.Where(a => a.Name == searchParam));
            List<Book> book = new List<Book>();
            foreach (var item in books)
            {
                if(item.Authors.Exists(a=>a.Name==searchParam))
                {
                    book.Add(item);
                }
            }
            ViewBag.Books = book;
            return View();
        }
    }
}
