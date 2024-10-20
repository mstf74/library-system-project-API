using BusinessLayer.Dto;
using BusinessLayer.ManagersInterfaces;
using DataAcessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace library_system_project_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksManager _booksManager;
        public BooksController(IBooksManager booksManager)
        {
            _booksManager = booksManager;
        }
        [HttpGet]
        public IActionResult GetAll() 
        {
            var books = _booksManager.GetAll();
            return Ok(books);
        }
        [HttpPost]
        public IActionResult AddBook(BookDto book)
        {
            var check = _booksManager.AddBook(book);
            if(check.IsValid)
                return Ok(book);
            return BadRequest(check.ValidationMessage);
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
        public IActionResult UpdateBook(int id, BookDto book) 
        {
            var check = _booksManager.UpdateBook(id, book);
            if (check.IsValid)
                return Ok(book);
            else
                return BadRequest(check.ValidationMessage);

        }



    }
}
