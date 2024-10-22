using BusinessLayer.Dto;
using BusinessLayer.ManagersInterfaces;
using DataAcessLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.StaticFiles;

namespace library_system_project_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BooksController : ControllerBase
    {
        private readonly IBooksManager _booksManager;
        private readonly string _uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        public BooksController(IBooksManager booksManager)
        {
            _booksManager = booksManager;
        }
        [HttpGet]
        //[AllowAnonymous]
        public IActionResult GetAll() 
        {
            var books = _booksManager.GetAll();
            return Ok(books);
        }
        [HttpGet("cover/{bookId}")]
        public IActionResult GetCover(int bookId)
        {
            var book = _booksManager.GetById(bookId);
            if (book is null)
                return BadRequest("image not found");
            var filePath = Path.Combine(_uploadsPath, book.CoverUrl??"");

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            // Detect the MIME type based on the file extension
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out string contentType))
            {
                contentType = "application/octet-stream"; // Fallback if unknown type
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, contentType);
        }
        [HttpPost]
        public async Task<IActionResult> AddBook([FromForm]BookDto book)
        {
            if (book.CoverUrl == null || book.CoverUrl.Length == 0)
                return BadRequest("No file uploaded.");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", book.CoverUrl.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await book.CoverUrl.CopyToAsync(stream);
            }

            var check = _booksManager.AddBook(book);
            if (!check.IsValid) 
                    return BadRequest(check.ValidationMessage);
                return Ok(book);
        }
        [HttpDelete]
        public IActionResult DeleteBook(int id) 
        {
            var check = _booksManager.RemoveBook(id);
            if (check.IsValid)
                return Ok("book is deleted successfully");
            else
                return BadRequest(check.ValidationMessage);
        }
        [HttpPut]
        public IActionResult UpdateBook(int id,[FromForm] BookDto book) 
        {
            var check = _booksManager.UpdateBook(id, book);
            if (check.IsValid)
                return Ok(book);
            else
                return BadRequest(check.ValidationMessage);

        }



    }
}
