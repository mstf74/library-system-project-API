using BusinessLayer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ManagersInterfaces
{
    public interface ILoanManager
    {
        public ValidationValues AddLoan(string userId, int bookId,double fineamount);
        public ValidationValues CheckLoan(int loanId);
        public List<LoanAdminDto> GetAll();
        public List<LoanUserDto> GetAllByUserId(string userId);

    }
}
