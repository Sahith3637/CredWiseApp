# CredWise Customer Management System

## Recent Updates
- **Loan Enquiry API**: Added new endpoint `POST /api/LoanEnquiry` for submitting loan enquiries with name, phone number, loan amount, and purpose.
- **API Logging**: Enhanced API logging to include user ID and user type for all API calls.
- **Loan Product Documents**: Added support for associating documents with specific loan applications through the `LoanApplicationId` field.
- **Get All Loan Applications**: Added new endpoint `GET /api/LoanApplication/all` in `LoanApplicationController` to retrieve all loan applications with user and product details.
- **Loan Application APIs**: Gold, Home, and Personal loan application APIs are now available in `LoanApplicationController`.
- **Repayment API**: The repayment response (`RepaymentScheduleDto`) now includes a `paymentType` property, reflecting the payment method used for each installment.
- **How It Works API**: Added `GET /api/HowItWorks` endpoint, returning the loan process steps as a list of `HowItWorksStepDto`.
- **Document Upload**: Added `POST /api/Loan/upload-loan-product-document` endpoint for uploading required documents for loan products.
- **DTO Documentation**: All DTOs for user, loan, repayment, FD, and document management are now documented in this file for easy reference.
- **API Endpoint Documentation**: All major endpoints are listed with their request/response DTOs and usage examples.
- **Payment History by User**: Added `GET /api/Repayment/user/{userId}/payment-history` endpoint to fetch all payment transactions for a user.
- **Meta Data Endpoints for Dropdowns**: Added `MetaController` with endpoints to fetch gender and employment type options for UI dropdowns. This allows the frontend to dynamically populate these fields instead of hardcoding them.

## Project Overview
CredWise Customer Management System is a .NET 8.0 based application that provides comprehensive user, loan, repayment, and document management functionality with secure authentication and authorization features.

## Project Structure
The solution is organized into multiple projects following Clean Architecture principles:

- **CredWiseCustomer.Api**: Web API project containing controllers and API endpoints
- **CredWiseCustomer.Application**: Application layer containing DTOs, interfaces, and business logic
- **CredWiseCustomer.Core**: Core domain entities and interfaces
- **CredWiseCustomer.Infrastructure**: Data access and external service implementations
- **CredWise.Utills**: Utility classes and helper functions
- **Authentication.Service**: Provides JWT authentication and policy-based authorization services
- **JwtSampleGenerator**: Console app to generate valid JWT tokens for testing the API

## Authentication & Authorization

### JWT Authentication
- The API uses JWT Bearer authentication.
- Tokens are generated using the `Authentication.Service` project, specifically the `JwtAuthenticationService` class.
- The JWT configuration (secret, issuer, audience) is set in `appsettings.json` and must match the values used in token generation.

#### Example JWT Configuration (appsettings.json):
```json
"Jwt": {
  "SecretKey": "A7d!kL9zQ2x#Vb6pT4w@eR8uY1s$Jm5n",
  "Issuer": "your-issuer",
  "Audience": "your-audience",
  "TokenExpirationInHours": 1
}
```

#### Claims Required for Admin Access
- `role`: Must be `"Admin"` (case-sensitive)
- `unique_name`: User's email
- `nameid`: User's ID

### Authorization
- Admin endpoints require the `role` claim to be `"Admin"`.
- Policies are defined in `Program.cs` using `RequireClaim("role", "Admin")`.
- The `[Authorize(Policy = "Admin")]` attribute is used on protected endpoints.

## How to Generate a JWT for Testing

1. **Build the Solution**
   - Ensure all projects build successfully. If you see errors about missing types, make sure `JwtSampleGenerator` references `Authentication.Service` and has the correct `using` statements.

2. **Run JwtSampleGenerator**
   - This console app will print a valid JWT token for an admin user.
   - Example usage:
     ```sh
     dotnet run --project JwtSampleGenerator
     ```
   - Output:
     ```
     Generated JWT token:
     <token>
     
     Use this token in your API requests as follows:
     Authorization: Bearer <token>
     ```

3. **Use the Token in API Requests**
   - Add the following header to your requests:
     ```
     Authorization: Bearer <token>
     ```
   - This is required for all protected endpoints.

## Troubleshooting Authentication & Authorization

### Common Issues
- **401 Unauthorized**: Token is missing, expired, invalid, or the `Bearer` prefix is missing in the header.
- **403 Forbidden**: Token is valid, but the user does not have the required role/claim for the endpoint.

### Debugging Steps
1. **Check the Authorization Header**
   - Must be exactly: `Authorization: Bearer <token>`
2. **Check JWT Claims**
   - Decode your JWT at [jwt.io](https://jwt.io/) and verify the `role` claim is exactly `"Admin"`.
3. **Check Policy and Controller Attributes**
   - Policy in `Program.cs` should be:
     ```csharp
     options.AddPolicy("Admin", policy => policy.RequireClaim("role", "Admin"));
     ```
   - Controller endpoints should use `[Authorize(Policy = "Admin")]`.
4. **Check Database User Role**
   - The `Role` column for your admin user in the database must be exactly `Admin` (case-sensitive, no spaces, not encoded).
   - Example SQL to fix:
     ```sql
     UPDATE Users SET Role = 'Admin' WHERE Email = 'admin@credwise.com';
     ```
5. **Check API Console Output**
   - Look for debug lines about claims and role value during authorization.
6. **Regenerate and Use a New Token**
   - After fixing the DB, log in again as admin to get a new token.

### Example: Generating and Using a Token

1. **Generate Token**
   - Run:
     ```sh
     dotnet run --project JwtSampleGenerator
     ```
   - Copy the output token.

2. **Use in API Request**
   - Example with curl:
     ```sh
     curl -H "Authorization: Bearer <token>" https://localhost:7037/api/User
     ```

## Common Mistakes
- Using a Base64-encoded or lowercase role value in the token or database (must be `Admin`).
- Not updating the token after fixing the database or code.
- Not using the `Bearer` prefix in the Authorization header.
- Policy and claim mismatch (policy expects `Admin`, token has something else).

---

## API Endpoints and Usage

### User Management
- **Create User**: `POST /api/User`
- **Get User by ID**: `GET /api/User/{id}`
- **Get User by Email**: `GET /api/User/email/{email}`
- **Get All Users**: `GET /api/User`
- **Update User**: `PUT /api/User/{id}`
- **Soft Delete User**: `DELETE /api/User/{id}`
- **Restore User**: `POST /api/User/{id}/restore`

### Loan Management (LoanController)
- **Apply for Loan**: `POST /api/Loan`
  - Request: `ApplyLoanDto` (see DTOs section)
  - Response: `LoanStatusDto`
- **Get Loan Status**: `GET /api/Loan/{loanApplicationId}`
- **Get All Loans for User**: `GET /api/Loan/user/{userId}`
- **Get All Loan Applications**: `GET /api/Loan/all`
  - Response: List of `LoanStatusDto` with user and product details
- **Upload Loan Product Document**: `POST /api/Loan/upload-loan-product-document` (multipart/form-data)
  - Request: `UploadLoanProductDocumentDto` (see DTOs section)
  - Response: `{ message: string }`

### Loan Application Management (LoanApplicationController)
- **Apply for Gold Loan**: `POST /api/LoanApplication/gold`
  - Request: `GoldLoanApplicationDto`
  - Response: `LoanApplicationResponseDto`
- **Apply for Home Loan**: `POST /api/LoanApplication/home`
  - Request: `HomeLoanApplicationDto`
  - Response: `LoanApplicationResponseDto`
- **Apply for Personal Loan**: `POST /api/LoanApplication/personal`
  - Request: `PersonalLoanApplicationDto`
  - Response: `LoanApplicationResponseDto`
- **Get Loan Application Status**: `GET /api/LoanApplication/{loanApplicationId}`
- **Get All Loan Applications for User**: `GET /api/LoanApplication/user/{userId}`
- **Get All Loan Applications**: `GET /api/LoanApplication/all`
  - Response: List of `LoanApplicationResponseDto` with all details

### Repayment Management
- **Get Repayment Schedule**: `GET /api/Repayment/schedule/{loanApplicationId}`
  - Response: List of `RepaymentScheduleDto` (includes `paymentType`)
- **Submit Repayment**: `POST /api/Repayment/pay`
  - Request: `SubmitPaymentDto`
  - Response: `{ message: string, paymentType: string }`
- **Get Payment History by User**: `GET /api/Repayment/user/{userId}/payment-history`
  - Response: List of `PaymentHistoryDto`

### Fixed Deposit (FD) Management
- **Apply for FD**: `POST /api/Fd`
- **Get FD Status**: `GET /api/Fd/{fdApplicationId}`
- **Get All FDs for User**: `GET /api/Fd/user/{userId}`
- **Get FD Payment Schedule**: `GET /api/Fd/payment-schedule/{fdApplicationId}`

### How It Works
- **Get Steps**: `GET /api/HowItWorks`
  - Response: List of `HowItWorksStepDto` (see below)

### Loan Enquiry Management
- **Submit Loan Enquiry**: `POST /api/LoanEnquiry`
  - Request: `LoanEnquiryRequestDto` (see DTOs section)
  - Response: `LoanEnquiryResponseDto`

### Meta Data (Dropdown Options)
- **Get Genders**: `GET /api/meta/genders`
  - Response: `[ { "value": "Male", "label": "Male" }, { "value": "Female", "label": "Female" }, { "value": "Other", "label": "Other" } ]`
- **Get Employment Types**: `GET /api/meta/employment-types`
  - Response: `[ { "value": "Salaried", "label": "Salaried" }, { "value": "Self-Employed", "label": "Self-Employed" } ]`

These endpoints are intended for use by the UI to populate dropdowns for gender and employment type, ensuring consistency and maintainability across the application.

---

## DTOs (Data Transfer Objects)

### User
- `CreateUserDto`: Email, Password, FirstName, LastName, PhoneNumber, CreatedBy
- `UpdateUserDto`: FirstName, LastName, PhoneNumber, IsActive, Password
- `LoginUserDto`: Email, Password

### Loan
- `ApplyLoanDto`: UserId, LoanProductId, RequestedAmount, RequestedTenure, Gender, DOB, Aadhaar, Address, Income, EmploymentType, CreatedBy, AdditionalDetails (dictionary for type-specific fields)
- `LoanStatusDto`: LoanapplicationId, UserId, LoanProductId, RequestedAmount, RequestedTenure, Status, DecisionDate, DecisionReason, CreatedAt
- `LoanProductDocumentDto`: LoanProductDocumentId, LoanProductId, DocumentName, IsActive
- `UploadLoanProductDocumentDto`: LoanProductId, LoanApplicationId (optional), DocumentName, File (IFormFile)

### Loan Application
- `GoldLoanApplicationDto`: UserId, LoanProductId, RequestedAmount, RequestedTenure, Gender, DOB, Aadhaar, Address, Income, EmploymentType, CreatedBy, GoldWeight, GoldPurity
- `HomeLoanApplicationDto`: UserId, LoanProductId, RequestedAmount, RequestedTenure, Gender, DOB, Aadhaar, Address, Income, EmploymentType, CreatedBy, PropertyAddress, DownPaymentPercentage
- `PersonalLoanApplicationDto`: UserId, LoanProductId, RequestedAmount, RequestedTenure, Gender, DOB, Aadhaar, Address, Income, EmploymentType, CreatedBy
- `LoanApplicationResponseDto`: LoanApplicationId, UserId, LoanProductId, RequestedAmount, RequestedTenure, Status, DecisionDate, DecisionReason, CreatedAt, GoldLoanDetails, HomeLoanDetails, PersonalLoanDetails

### Repayment
- `RepaymentScheduleDto`: RepaymentId, LoanApplicationId, InstallmentNumber, DueDate, PrincipalAmount, InterestAmount, TotalAmount, Status, PaymentType
- `SubmitPaymentDto`: RepaymentId, Amount
- `PaymentHistoryDto`: TransactionId, LoanApplicationId, RepaymentId, Amount, PaymentDate, PaymentMethod, Status, Reference

### Fixed Deposit (FD)
- `FdStatusDto`: FDApplicationId, UserId, FDTypeId, Amount, Duration, InterestRate, Status, MaturityDate, MaturityAmount, CreatedAt
- `FdPaymentScheduleDto`: Id, FdId, PaymentDate, Amount, Status, PaidDate, TransactionId

### How It Works
- `HowItWorksStepDto`: StepNumber, Title, Description

### Loan Enquiry
- `LoanEnquiryRequestDto`: Name, PhoneNumber, LoanAmount, LoanPurpose
- `LoanEnquiryResponseDto`: EnquiryId, Name, PhoneNumber, LoanAmountRequired, LoanPurpose, CreatedAt

### Meta Data (Dropdown Options)
- **Get Genders**: `GET /api/meta/genders`
  - Response: `[ { "value": "Male", "label": "Male" }, { "value": "Female", "label": "Female" }, { "value": "Other", "label": "Other" } ]`
- **Get Employment Types**: `GET /api/meta/employment-types`
  - Response: `[ { "value": "Salaried", "label": "Salaried" }, { "value": "Self-Employed", "label": "Self-Employed" } ]`

These endpoints are intended for use by the UI to populate dropdowns for gender and employment type, ensuring consistency and maintainability across the application.

---

## Example: How It Works API Response
```