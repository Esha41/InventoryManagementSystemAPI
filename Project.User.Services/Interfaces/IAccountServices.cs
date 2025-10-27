using Microsoft.AspNetCore.Identity.Data;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.Interfaces
{
    public interface IAccountServices
    {
        Task<APIOperationResponse<AuthenticatedResponse>> Login(
      LoginInformation loginInformation,
      CancellationToken cancellationToken = default);
        Task<APIOperationResponse<List<ClaimDto>>> GetRoleClaimsOnlyAsync();
        Task<APIOperationResponse<string>> ForgotPasswordAsync(ForgotPasswordDto request);
        Task<APIOperationResponse<string>> ResetPasswordAsync(ResetPasswordDto request);


    }
}
