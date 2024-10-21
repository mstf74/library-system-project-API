using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Dto;
using BusinessLayer.ManagersInterfaces;


namespace library_system_project_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanManager _loanManager;
        public LoansController(ILoanManager loanManager)
        {
            _loanManager = loanManager;
        }
        [HttpGet]
        public IActionResult GetAll() 
        {
            var laons = _loanManager.GetAll();
            return Ok(laons);
        }
        [HttpGet]
        public IActionResult GetAllByUserId(string id) 
        {
            var loans = _loanManager.GetAllByUserId(id);
            return Ok(loans);
        }
        [HttpPost]
        public IActionResult AddLone(string userId, int bookId,double fineAmount)
        {
            var result = _loanManager.AddLoan(userId, bookId,fineAmount);
            if (!result.IsValid)
            {
                return BadRequest(result.ValidationMessage);
            }
            return Ok(result.ValidationMessage);
        }
        [HttpPut]
        public IActionResult CheckLoan(int loanId)
        {
            var result = _loanManager.CheckLoan(loanId);
            if (result.IsValid) 
            {
                return Ok(result.ValidationMessage);
            }
            return NoContent();
        }
    }
}
