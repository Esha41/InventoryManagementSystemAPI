using Ettad.ResponseHandler.Models;
using Ettad.Services.DataTransferObject.Customer;
using Ettad.User.Services.DataTransferObject.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.Interfaces
{
    public interface IOrganization
    {
        Task<APIOperationResponse<OrganizationCreateModel>> UpdateOrganizationAsync(OrganizationCreateModel model);
        Task<APIOperationResponse<OrganizationCreateModel>> CreateOrganizationAsync(OrganizationCreateModel model);

        Task<APIOperationResponse<List<CustomerGetAllModel>>> GetAllOrganizationAsync();
        Task<APIOperationResponse<bool>> DeleteOrganizationAsync(int id);
    }
}
