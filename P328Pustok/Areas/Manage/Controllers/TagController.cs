using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P328Pustok.DAL;
using P328Pustok.Models;
using P328Pustok.ViewModels;

namespace P328Pustok.Areas.Manage.Controllers
{
    [Area("manage")]
    public class TagController:Controller
    {
        private readonly PustokContext _context;

        public TagController(PustokContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.Tags.AsQueryable();

            if (search != null)
                query = query.Where(x => x.Name.Contains(search));

            ViewBag.Search = search;

            return View(PaginatedList<Tag>.Create(query, page, 3));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            if (!ModelState.IsValid)
                return View();

            if (_context.Tags.Any(x => x.Name == tag.Name))
            {
                ModelState.AddModelError("Name", "Name is already taken");
                return View();
            }

            _context.Tags.Add(tag);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Tag tags = _context.Tags.Find(id);

            if (tags == null) return View("Error");

            return View(tags);
        }

        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            if (!ModelState.IsValid) return View();

            Tag existTag = _context.Tags.Find(tag.Id);

            if (existTag == null) return View("Error");

            if (tag.Name != existTag.Name && _context.Tags.Any(x => x.Name == tag.Name))
            {
                ModelState.AddModelError("Name", "Name is already taken");
                return View();
            }

            existTag.Name = tag.Name;

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Tag tag = _context.Tags.Include(x => x.books).FirstOrDefault(x => x.Id == id);

            if (tag == null) return View("Error");
            return View(tag);
        }

        [HttpPost]
        public IActionResult Delete(Tag tag)
        {
            Tag existTag = _context.Tags.Find(tag.Id);

            if (existTag == null) return View("Error");

            _context.Tags.Remove(existTag);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
