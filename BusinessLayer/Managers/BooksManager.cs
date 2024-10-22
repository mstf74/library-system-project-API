using BusinessLayer.Dto;
using BusinessLayer.ManagersInterfaces;
using DataAcessLayer.GenericRepo;
using DataAcessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers
{
    public class BooksManager:IBooksManager
    {
        private readonly IGenericRepo<Book> _bookRepo;
        public BooksManager(IGenericRepo<Book> bookRepo)
        {
            _bookRepo = bookRepo;
        }

        public List<Book> GetAll()
        {
            var books = _bookRepo.getAll().ToList();
            return books;
        }
        public ValidationValues AddBook(BookDto book)
        {
            var nameCheck = checkUniqueNames(book);
            if (nameCheck != null)
                return nameCheck;

            Book newbook = new Book()
            {
                Title = book.Title,
                Author = book.Author,
                publicationYear = book.publicationYear,
                Category = book.Category,
                NumberOfCopies = book.NumberOfCopies,
                AvailableCopies = book.AvailableCopies,
                CoverUrl = book.CoverUrl.FileName,
            };
            var result = validateException(_bookRepo.add, newbook);
            return result;
        }


        public ValidationValues RemoveBook(int bookId)
        {
            bool check;
            try
            {
                 check = _bookRepo.remove(bookId);
            }
            catch (Exception ex) 
            {
                return new ValidationValues()
                {
                    ValidationMessage = ex.Message,
                    IsValid = false,
                };
            }
            if (!check)
            {
                return new ValidationValues()
                {
                    ValidationMessage = "book wasn't found",
                    IsValid = false
                };
            }
            return new  ValidationValues(){
                IsValid = check
            };
        }

        public ValidationValues UpdateBook(int bookId,BookDto book)
        {
            var nameCheck = checkUniqueNames(book);
            if(nameCheck != null)
                return nameCheck;

            Book oldBook = _bookRepo.getById(bookId);
            if (oldBook == null) 
            {
                return new ValidationValues(){
                    ValidationMessage="book wasn't found",
                    IsValid = false
                };
            }
            oldBook.Title = book.Title;
            oldBook.Author = book.Author;
            oldBook.publicationYear = book.publicationYear;
            oldBook.Category = book.Category;
            oldBook.NumberOfCopies = book.NumberOfCopies;
            oldBook.AvailableCopies = book.AvailableCopies;
            oldBook.CoverUrl = book.CoverUrl.FileName;
            var result = validateException(_bookRepo.update,oldBook);
            return result;
        }
        public  Book GetById(int id)
                 => _bookRepo.getById(id);

        

        private ValidationValues checkUniqueNames(BookDto book)
        {
            var nameCheck = _bookRepo.getAllFilter(e => e.Title == book.Title && e.Author == book.Author).FirstOrDefault();
            if (nameCheck != null )
            {
                return new ValidationValues()
                {
                    ValidationMessage = "The book title and author must be unique for each book",
                    IsValid = false
                };
            }
            return null;
        } 
        private ValidationValues validateException(Func<Book, bool> fun,Book book)
        {
            bool check;
            try
            {
                check = fun(book);
            }
            catch (Exception ex) 
            {
                return new ValidationValues()
                {
                    ValidationMessage = ex.Message,
                    IsValid = false
                };
            }
            return new ValidationValues()
            {
                IsValid = check
            };

        }


    }
}
