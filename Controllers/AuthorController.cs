using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Test_Midterm.Data;

namespace Test_Midterm.Controllers
{
    public class AuthorController : Controller
    {
        private readonly TestMidtermContext _context;

        public AuthorController(TestMidtermContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var authors = _context.Authors.ToList();
            return View(authors.ToList());
        }

        #region create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Author = new SelectList(_context.Authors, "AuthorId", "Name", "Bio");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Authors.Add(author);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Author = new SelectList(_context.Authors, "AuthorId", "Name", "Bio");
            return View(author);
        }
        #endregion
        #region edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = _context.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }
            ViewBag.Author = new SelectList(_context.Authors, "AuthorId", "Name", "Bio");
            return View(author);
        }
        [HttpPost]
        public IActionResult Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Authors.Update(author);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Author = new SelectList(_context.Authors, "AuthorId", "Name", "Bio");
            return View(author);
        }
        #endregion
        #region delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var author = _context.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var author = _context.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }
            _context.Authors.Remove(author);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
        #region BooksByAuthor
        public IActionResult BooksByAuthor()
        {
            var books = _context.Books
            .GroupBy(b => new { b.AuthorId, b.Author.Name })
            .Select(g => new
            {
            AuthorId = g.Key.AuthorId,
            AuthorName = g.Key.Name,
            BookCount = g.Count()
            });
            return View(books.ToList());
        }
        #endregion
    }
}