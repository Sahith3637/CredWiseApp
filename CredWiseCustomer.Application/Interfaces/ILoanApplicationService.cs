using CredWiseCustomer.Application.DTOs;

namespace CredWiseCustomer.Application.Interfaces;

public interface ILoanApplicationService
{
    Task<LoanApplicationResponseDto> ApplyForLoanAsync(ApplyLoanDto dto);
    Task<LoanApplicationResponseDto> GetLoanApplicationStatusAsync(int loanApplicationId);
    Task<IEnumerable<LoanApplicationResponseDto>> GetUserLoanApplicationsAsync(int userId);
    Task<IEnumerable<LoanApplicationResponseDto>> GetAllLoanApplicationsAsync();
} 