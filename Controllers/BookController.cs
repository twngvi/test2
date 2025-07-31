using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Test_Midterm.Data;
using Test_Midterm.Helper;

namespace Test_Midterm.Controllers
{
    public class BookController : Controller
    {
        public readonly TestMidtermContext _context;
        public BookController(TestMidtermContext context)
        {
            _context = context;
        }
        public IActionResult Index(string keyword)
        {
            var Book = _context.Books.Include(p => p.Author).AsQueryable().ToList();
            if (keyword != null)
			{
                Book = Book.Where(p => p.Title.Contains(keyword) || p.Description.Contains(keyword) || p.Author.Name.Contains(keyword)|| p.CoverImagePath.Contains(keyword)).ToList();
			}
            return View(Book.ToList());
        }

        #region create
        [HttpGet]
        public IActionResult create() {
            ViewBag.Author = new SelectList(_context.Authors, "AuthorId", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult create(Book model, IFormFile image)
        {
            if (image != null)
            {
                model.CoverImagePath = MyTool.UploadImageToFolder(image, "Book");
            }
            else
            {
                model.CoverImagePath = null;
            }
            try
            {
                _context.Books.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index","Book");
            }
            catch 
            {
                ViewBag.Message = "Có lỗi";
                ViewBag.Author = new SelectList(_context.Authors, "AuthorId", "Name");
                return View(model);
            }
        }
        #endregion

        #region edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.Author = new SelectList(_context.Authors, "AuthorId", "Name");
            return View(book);
        }
        [HttpPost]
        public IActionResult Edit(Book model, IFormFile image, int id)
        {
            var book = _context.Books.SingleOrDefault(p => p.BookId== id);
            if (book == null)
            {
                return NotFound();
            }

            book.AuthorId = model.AuthorId;
            book.Title = model.Title;
            book.Description = model.Description;

            if (image != null)
            {
                book.CoverImagePath = MyTool.UploadImageToFolder(image, "Book");
            }
            try
            {
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Có lỗi";
                ViewBag.Author = new SelectList(_context.Authors, "AuthorId", "Name");
                return View(model);
            }
        }
        #endregion
        #region Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var book = _context.Books.Include(b => b.Author).FirstOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            try
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Có lỗi";
                return View(book);
            }
        }
        #endregion
    }
}
