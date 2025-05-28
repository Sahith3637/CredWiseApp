using AutoMapper;
using CredWiseCustomer.Application.DTOs;
using CredWiseCustomer.Core.Entities;

namespace CredWiseCustomer.Application.Mappings;

public class LoanApplicationProfile : Profile
{
    public LoanApplicationProfile()
    {
        CreateMap<LoanApplication, LoanApplicationResponseDto>();
    }
} 