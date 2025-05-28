using AutoMapper;
using CredWiseCustomer.Application.DTOs;
using CredWiseCustomer.Application.Interfaces;
using CredWiseCustomer.Core.Entities;

namespace CredWiseCustomer.Application.Services
{
    public class LoanEnquiryService : ILoanEnquiryService
    {
        private readonly ILoanEnquiryRepository _loanEnquiryRepository;
        private readonly IMapper _mapper;

        public LoanEnquiryService(ILoanEnquiryRepository loanEnquiryRepository, IMapper mapper)
        {
            _loanEnquiryRepository = loanEnquiryRepository;
            _mapper = mapper;
        }

        public async Task<LoanEnquiryResponseDto> AddEnquiryAsync(LoanEnquiryRequestDto dto)
        {
            var enquiry = _mapper.Map<LoanEnquiry>(dto);
            enquiry.CreatedAt = DateTime.UtcNow;
            var result = await _loanEnquiryRepository.AddAsync(enquiry);
            return _mapper.Map<LoanEnquiryResponseDto>(result);
        }
    }
} 