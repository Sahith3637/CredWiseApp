using AutoMapper;
using CredWiseCustomer.Application.DTOs;
using CredWiseCustomer.Application.Interfaces;
using CredWiseCustomer.Core.Entities;


namespace CredWiseCustomer.Application.Services;

public class LoanApplicationService : ILoanApplicationService
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly IMapper _mapper;

    public LoanApplicationService(ILoanApplicationRepository loanApplicationRepository, IMapper mapper)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _mapper = mapper;
    }

    public async Task<LoanApplicationResponseDto> ApplyForLoanAsync(ApplyLoanDto dto)
    {
        var loanProduct = await _loanApplicationRepository.GetLoanProductByIdAsync(dto.LoanProductId);
        if (loanProduct == null)
            throw new KeyNotFoundException($"Loan product with ID {dto.LoanProductId} not found");

        // Create base loan application
        var loanApplication = new LoanApplication
        {
            UserId = dto.UserId,
            LoanProductId = dto.LoanProductId,
            RequestedAmount = dto.RequestedAmount,
            RequestedTenure = dto.RequestedTenure,
            Gender = dto.Gender,
            Dob = DateOnly.FromDateTime(dto.DOB),
            Aadhaar = dto.Aadhaar,
            Address = dto.Address,
            Income = dto.Income,
            EmploymentType = dto.EmploymentType,
            Status = "Initial Review",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = dto.CreatedBy
        };
        await _loanApplicationRepository.AddAsync(loanApplication);

        // Handle type-specific details
        if (loanProduct.LoanType == "GOLD")
        {
            var goldWeight = Convert.ToDecimal(dto.AdditionalDetails["GoldWeight"]);
            var goldPurity = dto.AdditionalDetails["GoldPurity"].ToString();
            var goldLoanApplication = new GoldLoanApplication
            {
                LoanApplication = loanApplication,
                GoldWeight = goldWeight,
                GoldPurity = goldPurity
            };
            await _loanApplicationRepository.AddGoldLoanApplicationAsync(goldLoanApplication);
        }
        else if (loanProduct.LoanType == "HOME")
        {
            var propertyAddress = dto.AdditionalDetails["PropertyAddress"].ToString();
            var downPayment = Convert.ToDecimal(dto.AdditionalDetails["DownPaymentPercentage"]);
            var homeLoanApplication = new HomeLoanApplication
            {
                LoanApplication = loanApplication,
                PropertyAddress = propertyAddress,
                DownPaymentPercentage = downPayment
            };
            await _loanApplicationRepository.AddHomeLoanApplicationAsync(homeLoanApplication);
        }
        // For PERSONAL, no extra details needed

        return _mapper.Map<LoanApplicationResponseDto>(loanApplication);
    }

    public async Task<LoanApplicationResponseDto> GetLoanApplicationStatusAsync(int loanApplicationId)
    {
        var application = await _loanApplicationRepository.GetByIdAsync(loanApplicationId);
        if (application == null)
            throw new KeyNotFoundException($"Loan application with ID {loanApplicationId} not found");

        return _mapper.Map<LoanApplicationResponseDto>(application);
    }

    public async Task<IEnumerable<LoanApplicationResponseDto>> GetUserLoanApplicationsAsync(int userId)
    {
        var applications = await _loanApplicationRepository.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<LoanApplicationResponseDto>>(applications);
    }

    public async Task<IEnumerable<LoanApplicationResponseDto>> GetAllLoanApplicationsAsync()
    {
        var applications = await _loanApplicationRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<LoanApplicationResponseDto>>(applications);
    }
} 