using BookLibrary.Data;
using BookLibrary.Data.Entities;
using BookLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AuthorController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            var authors = _applicationDbContext.Authors.ToList();
            return View(authors);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Author author)
        {
            if (author == null)
                return NotFound();
            _applicationDbContext.Authors.Add(author);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var authorForEdit = _applicationDbContext.Authors.Find(id);
            return View(authorForEdit);
        }
        [HttpPost]
        public IActionResult Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Authors.Update(author);
                _applicationDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var deletedAuthor = _applicationDbContext.Authors.Find(id);

            _applicationDbContext.Authors.Remove(deletedAuthor!);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Search(string searchParam)
        {
            if (searchParam == null)
                return NotFound();
            var books = _applicationDbContext.Authors.Where(n=>n.Name==searchParam);
            ViewBag.Books = books;
            return View();
        }
    }
}
