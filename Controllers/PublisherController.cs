using BookLibrary.Data;
using BookLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    public class PublisherController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public PublisherController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            var publishers = _applicationDbContext.Publishers.ToList();
            return View(publishers);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Publisher publisher)
        {   if (ModelState.IsValid)
            {
                _applicationDbContext.Publishers.Add(publisher);
                _applicationDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var editPublisher = _applicationDbContext.Publishers.Find(id);
            return View(editPublisher);
        }
        [HttpPost]
        public IActionResult Edit(Publisher publisher)
        {
            _applicationDbContext.Publishers.Update(publisher);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
