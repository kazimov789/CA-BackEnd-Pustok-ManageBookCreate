using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P328Pustok.DAL;
using P328Pustok.Models;

namespace P328Pustok.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AuthorController : Controller
    {
        private readonly PustokContext _context;

        public AuthorController(PustokContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Authors.Include(x=>x.Books).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Author author)
        {
            if (!ModelState.IsValid)
                return View();

            if (_context.Authors.Any(x => x.FullName == author.FullName))
            {
                ModelState.AddModelError("FullName", "Name is already taken");
                return View();
            }
            _context.Authors.Add(author);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Author author = _context.Authors.Find(id);

            if (author == null) return View("Error");

            return View(author);
        }

        [HttpPost]
        public IActionResult Edit(Author author)
        {
            if (!ModelState.IsValid) return View();

            Author existAuthor = _context.Authors.Find(author.Id);

            if (existAuthor == null) return View("Error");

            if (author.FullName != existAuthor.FullName && _context.Authors.Any(x => x.FullName == author.FullName))
            {
                ModelState.AddModelError("FullName", "THis Author  is already have");
                return View();
            }

            existAuthor.FullName = author.FullName;

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Author author = _context.Authors.Include(x => x.Books).FirstOrDefault(x => x.Id == id);

            if (author == null) return View("Error");
            return View(author);
        }

        [HttpPost]
        public IActionResult Delete(Author author)
        {
            Author existAuthor = _context.Authors.Find(author.Id);

            if (existAuthor == null) return View("Error");

            _context.Authors.Remove(existAuthor);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
