using CredWiseCustomer.Application.DTOs;
using CredWiseCustomer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CredWiseCustomer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanEnquiryController : ControllerBase
    {
        private readonly ILoanEnquiryService _loanEnquiryService;

        public LoanEnquiryController(ILoanEnquiryService loanEnquiryService)
        {
            _loanEnquiryService = loanEnquiryService;
        }

        [HttpPost]
        public async Task<ActionResult<LoanEnquiryResponseDto>> SubmitEnquiry([FromBody] LoanEnquiryRequestDto dto)
        {
            var result = await _loanEnquiryService.AddEnquiryAsync(dto);
            return Ok(result);
        }
    }
} 