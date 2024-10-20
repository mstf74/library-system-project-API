using BusinessLayer.Dto;
using DataAcessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ManagersInterfaces
{
    public interface IBooksManager
    {
        public List<Book> GetAll();
        public ValidationValues AddBook(BookDto book);
        public ValidationValues RemoveBook(int bookId);
        public ValidationValues UpdateBook(int bookId, BookDto book);
    }
}
