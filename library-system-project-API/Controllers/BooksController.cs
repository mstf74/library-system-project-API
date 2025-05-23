﻿using BusinessLayer.Dto;
using BusinessLayer.ManagersInterfaces;
using DataAcessLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace library_system_project_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class BooksController : ControllerBase
    {
        private readonly IBooksManager _booksManager;
        private readonly string _uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        private readonly string[] extenstions = { ".png", ".jpg" };

        public BooksController(IBooksManager booksManager)
        {
            _booksManager = booksManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var path = $"{Request.Scheme}://{Request.Host}/uploads/";
            var books = _booksManager.GetAll(path);
            return Ok(books);
        }
        [HttpPost]
        public async Task<IActionResult> AddBook([FromForm] BookDto book)
        {
            if (book.CoverUrl == null || book.CoverUrl.Length == 0)
                return BadRequest("No file uploaded.");

            var filePath = _uploadsPath;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var imageName = GenerateRandomFileName(book.CoverUrl);
            if (imageName is null)
                return BadRequest("invalid file extension");
            string imagePath = Path.Combine(filePath, imageName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await book.CoverUrl.CopyToAsync(stream);
            }
            book.CoverName = imageName;
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
        public async Task<IActionResult> UpdateBook(int id, [FromForm] BookDto book)
        {
            if (book.CoverUrl == null || book.CoverUrl.Length == 0)
                return BadRequest("No file uploaded.");
            var imageName = GenerateRandomFileName(book.CoverUrl);
            if (imageName is null)
                return BadRequest("invalid file extension");
            var filePath = Path.Combine(_uploadsPath, imageName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await book.CoverUrl.CopyToAsync(stream);
            }
            book.CoverName = imageName;
           
            var check = _booksManager.UpdateBook(id, book);
            if (check.IsValid)
                return Ok("Book Updated");
            else
                return BadRequest(check.ValidationMessage);
        }
        private string GenerateRandomFileName(IFormFile file)
        {
            string imageExtension = Path.GetExtension(file.FileName);
            if (!extenstions.Contains(imageExtension))
                return null;
            return Guid.NewGuid().ToString() + imageExtension;
        }
    }
}
