using BusinessLayer.Dto;
using BusinessLayer.ManagersInterfaces;
using DataAcessLayer.GenericRepo;
using DataAcessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers
{
    public class LoanManager:ILoanManager
    {
        private readonly IGenericRepo<Loan> _loanRepo;
        private readonly IGenericRepo<Book> _bookRepo;
        private readonly IGenericRepo<Fine> _fineRepo;
        UserManager<ApplicationUser> _UserManager;

        public LoanManager(IGenericRepo<Loan> loanRepo, IGenericRepo<Book> bookRepo
            , UserManager<ApplicationUser> UserManager, IGenericRepo<Fine> fineRepo)
        {
            _loanRepo = loanRepo;
            _bookRepo = bookRepo;
            _UserManager = UserManager;
            _fineRepo = fineRepo;
        }
        public List<LoanAdminDto> GetAll()
        {
            var laons = _loanRepo.getAllFilter(include: e=>e.Include(e=>e.User)).Select(e=>new LoanAdminDto
            {
                Id = e.Id,
                BookId = e.BookId,
                UserName = e.User.UserName,
                Status = e.Status,
                LoanDate = e.LoanDate.ToString("yyyy-MM-dd"),
                ReturnDate = e.ReturnDate.ToString("yyyy-MM-dd")
            }).ToList();
            return laons;
        }
        public List<LoanUserDto> GetAllByUserId(string userId) 
        {
            var laons = _loanRepo.getAllFilter(filter:e=>e.UserId==userId,
                include: e => e.Include(e => e.Book)).Select(e => new LoanUserDto
            {
                BookName = e.Book.Title,   
                LoanDate = e.LoanDate.ToString("yyyy-MM-dd"),
                ReturnDate = e.ReturnDate.ToString("yyyy-MM-dd")
            }).ToList();
            return laons;
        }
        public ValidationValues AddLoan(string userId, int bookId,double fineAmount)
        {
            var book = _bookRepo.getById(bookId);
            var user = _UserManager.FindByIdAsync(userId).Result;
            if (user== null) 
            {
                return new ValidationValues()
                {
                    ValidationMessage = " User is not found",
                    IsValid = false
                };

            }
            if (book == null) 
            {
                return new ValidationValues()
                {
                    ValidationMessage = " Can't find the book",
                    IsValid = false
                };
            }
            if(book.AvailableCopies== 0)
            {
                return new ValidationValues()
                {
                    ValidationMessage = "No available Copy",
                    IsValid = false
                };
                
            }
            if (fineAmount < 20)
            {
                return new ValidationValues()
                {
                    ValidationMessage = "not enought fine",
                    IsValid = false
                };
            }
            Loan loan = new Loan()
            {
                BookId = bookId,
                UserId = userId,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14),
                ReturnDate = DateTime.Now.AddDays(14),
                Status ="Loaned"
            };
            var check = _loanRepo.add(loan);
            if (check)
            {
                book.AvailableCopies -= 1;
                _bookRepo.update(book);
                Fine fine = new Fine()
                {
                    FineAmount = fineAmount,
                    PaymentStatus = "success",
                    IssueDate = DateTime.Now,
                    LoanId = loan.Id,
                    UserId = userId,
                };
                _fineRepo.add(fine);

                return new ValidationValues()
                {
                    IsValid = true,
                    ValidationMessage= "the book is loaned Successfully"
                };
            }
            return new ValidationValues()
            {
                ValidationMessage = "couldn't loan the book",
                IsValid = false
            };
        }
        public ValidationValues CheckLoan(int loanId)
        {
            var oldloan = _loanRepo.getById(loanId);
            if (oldloan == null)
            {
                return new ValidationValues()
                {
                    ValidationMessage = "couldn't find the loan",
                    IsValid = false
                };
            }
            var book = _bookRepo.getById(oldloan.BookId);
            if (DateTime.Now >= oldloan.ReturnDate)
            {
                oldloan.ReturnDate = DateTime.Now;
                oldloan.Status = "Returned";
                book.AvailableCopies += 1;
            }
            var check = _loanRepo.update(oldloan);
            if (check) 
            {
                return new ValidationValues()
                {
                    IsValid = true,
                    ValidationMessage = "the row is refreshed"
                };
            }
            return new ValidationValues();
            

        }
    }
}
